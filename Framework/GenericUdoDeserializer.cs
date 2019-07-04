// <copyright filename="GenericUdoDeserializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using B1PP.Database;
using B1PP.Database.Attributes;
using B1PP.Exceptions;
using B1PP.Extensions.Common;
using SAPbobsCOM;

namespace B1PP
{
    internal sealed class GenericUdoDeserializer<T> : IUdoDeserializer<T> where T : class, new()
    {
        private bool IsSystemTimeProperty(PropertyInfo property)
        {
            return property.HasAttribute<SystemFieldAttribute>() &&
                   (@"CreateTime".Equals(property.Name) || @"UpdateTime".Equals(property.Name));
        }

        private static bool IsUserTimeProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<UserFieldAttribute>()?.SubType != BoFldSubTypes.st_Time;
        }
        
        private object DefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        
        private object ConvertToObject(string value, PropertyInfo property)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DefaultValue(property.PropertyType);
            }

            if (property.PropertyType == typeof(string))
            {
                return value;
            }

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (IsUserTimeProperty(property) || IsSystemTimeProperty(property))
                {
                    return Convert.ToInt32(value.Replace(@":", string.Empty), CultureInfo.InvariantCulture);
                }

                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }

            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }

            throw new ArgumentException($@"Property '{property.Name}' is of an unsupported type '{property.PropertyType}'");
        }

        private bool HasSystemOrUserField(PropertyInfo p)
        {
            return p.HasAttribute<UserFieldAttribute>() || p.HasAttribute<SystemFieldAttribute>();
        }       
        
        private Func<PropertyInfo, bool> IsChild
        {
            get
            {
                return p => p.GetCustomAttribute<ChildrenAttribute>() != null;
            }
        }
        
        private IEnumerable<PropertyInfo> GetSystemAndUserFields()
        {
            return typeof(T).GetProperties().Where(HasSystemOrUserField);
        }
        
        private List<PropertyInfo> ChildSystemAndUserFields(Type childType)
        {
            return childType.GetProperties().Where(HasSystemOrUserField).ToList();
        }

        private void SetCollectionProperty(PropertyInfo child, T instance, XDocument document)
        {
            var collection = child.GetValue(instance);
            var childType = child.PropertyType.GenericTypeArguments[0];
            string childObject = childType.GetCustomAttribute<ChildUserTableAttribute>().ObjectName;
            var childProperties = ChildSystemAndUserFields(childType);

            var detail = document.XPathSelectElements($@"//{childObject}");
            foreach (var element in detail)
            {
                var childInstance = Activator.CreateInstance(childType);
                foreach (var childProperty in childProperties)
                    SetProperty(childProperty, element, childInstance);

                var add = child.PropertyType.GetMethod(@"Add");
                if (add == null)
                    throw new InvalidOperationException($@"Property '{child.Name}' is not a collection type.");

                add.Invoke(collection, new[] {childInstance});
            }
        }
        
        
        private void SetProperty(PropertyInfo property, XElement element, object instance)
        {
            string fieldName = property.GetFieldName();
            string xmlValue = element.Element($@"{fieldName}")?.Value ?? string.Empty;

            var value = ConvertToObject(xmlValue, property);
            var setter = property.DeclaringType?.GetProperty(property.Name)?.GetSetMethod(true);
            setter?.Invoke(instance, new[] {value});
        }
        
        private IEnumerable<PropertyInfo> GetChildrenProperties()
        {
            return typeof(T).GetProperties().Where(IsChild);
        }

        /// <summary>
        /// Deserializes xml into an object instance of the specified type.
        /// </summary>
        /// <param name="xml">The serialized xml of an instance of the specified type.</param>
        /// <returns>An instance of the specified type.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the xml is null or empty.
        /// </exception>
        public T Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException(@"The xml string cannot be null or empty.", nameof(xml));

            var userObjectAttribute = typeof(T).GetCustomAttribute<UserObjectAttribute>();
            if (userObjectAttribute == null)
                throw MissingAttributeException.Create(typeof(T), typeof(UserObjectAttribute));

            string objectName = userObjectAttribute.ObjectName;
            var document = XDocument.Parse(xml);
            var xmlData = document.XPathSelectElement($@"//{objectName}");

            var instance = new T();
            var properties = GetSystemAndUserFields();
            foreach (var property in properties)
                SetProperty(property, xmlData, instance);

            var children = GetChildrenProperties();
            foreach (var child in children)
                SetCollectionProperty(child, instance, document);

            return instance;
        }
    }
}
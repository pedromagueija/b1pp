// <copyright filename="GenericUdoSerializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System.Collections;
using B1PP.Database;
using B1PP.Extensions.Common;

namespace B1PP
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Database.Attributes;

    using Exceptions;

    /// <summary>
    /// Serializes and de-serializes user defined object to and from XML.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the serialized/deserialized object.
    /// </typeparam>
    public class GenericUdoSerializer<T> : IUdoSerializer<T> where T : class
    {
        /// <summary>
        /// Serializes the given instance.
        /// </summary>
        /// <returns>
        /// An XML representation of the object.
        /// </returns>
        public string Serialize(T instance)
        {
            var userObjectAttribute = typeof(T).GetCustomAttribute<UserObjectAttribute>();
            if (userObjectAttribute == null)
                throw MissingAttributeException.Create(typeof(T), typeof(UserObjectAttribute));
            
            var type = instance.GetType();
            var xmlProperties = GetXmlProperties(instance);
            var childXmlProperties = GetChildXmlProperties(type, instance);

            var content = new XElement(userObjectAttribute.ObjectName, xmlProperties, childXmlProperties);

            var document = new XDocument(content);
            return document.ToString(SaveOptions.DisableFormatting);
        }
        
        private Func<PropertyInfo, bool> IsChild
        {
            get
            {
                return p => p.GetCustomAttribute<ChildrenAttribute>() != null;
            }
        }
        
        private XElement GetChildCollectionElement(PropertyInfo property, T instance)
        {
            string name = GetChildObjectName(property);

            var values = (IEnumerable) property.GetValue(instance);
            var childCollection = new XElement($@"{name}Collection");

            foreach (var value in values)
                childCollection.Add(new XElement(name, GetXmlProperties(value)));

            return childCollection;
        }
        
        private string GetChildObjectName(PropertyInfo property)
        {
            var child = property.GetCustomAttribute<ChildrenAttribute>();
            var attr = child.Type.GetCustomAttribute<ChildUserTableAttribute>();
        return                                                                                                                                                                                                                                                                                                                                                                                                                 attr.ObjectName;
        }
        
        private IEnumerable<XElement> GetChildXmlProperties(Type t, T instance)
        {
            var properties = t.GetProperties().Where(IsChild);

            foreach (var property in properties)
                yield return GetChildCollectionElement(property, instance);
        }        

        private string ConvertToString(PropertyInfo property, object instance)
        {
            var value = property.GetValue(instance);
            if (value == null)
                return null;

            // note that the value itself cannot be null at this point
            if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
            {
                var date = (DateTime) value;
                if (date < new DateTime(32, 01, 01)) // the sdk has an issue when the date is before the year 32
                    return string.Empty;

                return date.ToString(@"yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            if (property.PropertyType.IsEnum)
                return Enum.Format(property.PropertyType, value, @"d");

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private string GetFieldName(PropertyInfo property)
        {
            return new GetFieldName().FromProperty(property);
        }

        private IEnumerable<PropertyInfo> GetSerializableProperties(Type t)
        {
            return t.GetProperties().Where(p => IsField(p) && p.CanWrite);
        }

        private IEnumerable<XElement> GetXmlProperties(object instance)
        {
            var type = instance.GetType();
            var properties = GetSerializableProperties(type);

            foreach (var property in properties)
                if (ConvertToString(property, instance) != null)
                    yield return new XElement(GetFieldName(property), ConvertToString(property, instance));
        }     
        
        private bool IsField(PropertyInfo p)
        {
            return p.HasAttribute<UserFieldAttribute>() || p.HasAttribute<SystemFieldAttribute>();
        }    
    }
}
// <copyright filename="GenericUdoSerializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Database;
    using Database.Attributes;

    using Exceptions;

    using SAPbobsCOM;

    /// <summary>
    /// Serializes and de-serializes user defined object to and from XML.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the serialized/deserialized object.
    /// </typeparam>
    public class GenericUdoSerializer<T> where T : new()
    {
        private Func<PropertyInfo, bool> IsChild
        {
            get
            {
                return p => p.GetCustomAttribute<ChildrenAttribute>() != null;
            }
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

        private object ConvertToObject(XElement fieldNode, PropertyInfo property)
        {
            if (string.IsNullOrEmpty(fieldNode?.Value))
                return DefaultValue(property.PropertyType);

            if (property.PropertyType == typeof(string))
                return fieldNode.Value;

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (IsUserTimeProperty(property) || IsSystemTimeProperty(property))
                {
                    return Convert.ToInt32(fieldNode.Value.Replace(@":", string.Empty), CultureInfo.InvariantCulture);
                }

                return Convert.ToInt32(fieldNode.Value, CultureInfo.InvariantCulture);
            }

            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                return Convert.ToDateTime(fieldNode.Value, CultureInfo.InvariantCulture);

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                return Convert.ToDouble(fieldNode.Value, CultureInfo.InvariantCulture);

            throw new ArgumentException(
                $@"Property '{property.Name}' is of an unsupported type '{property.PropertyType}'");
        }
        

        private object ConvertToObject(string value, PropertyInfo property)
        {
            if (string.IsNullOrEmpty(value))
                return DefaultValue(property.PropertyType);

            if (property.PropertyType == typeof(string))
                return value;

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (IsUserTimeProperty(property) || IsSystemTimeProperty(property))
                {
                    return Convert.ToInt32(value.Replace(@":", string.Empty), CultureInfo.InvariantCulture);
                }

                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }

            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                return Convert.ToDateTime(value, CultureInfo.InvariantCulture);

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);

            throw new ArgumentException(
                $@"Property '{property.Name}' is of an unsupported type '{property.PropertyType}'");
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

                return date.ToString(@"yyyy-MM-dd");
            }

            if (property.PropertyType.IsEnum)
                return Enum.Format(property.PropertyType, value, @"d");

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private object DefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
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
            return attr.ObjectName;
        }

        private IEnumerable<PropertyInfo> GetChildrenProperties()
        {
            return typeof(T).GetProperties().Where(IsChild);
        }

        private IEnumerable<XElement> GetChildXmlProperties(Type t, T instance)
        {
            var properties = t.GetProperties().Where(IsChild);

            foreach (var property in properties)
                yield return GetChildCollectionElement(property, instance);
        }

        /// <summary>
        /// Gets the field name.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The field name.
        /// </returns>
        /// <remarks>
        /// The field name is extracted from the <see cref="FieldNameAttribute" /> if one exists.
        /// This allows us to name the properties correctly even when field names are abbreviated.
        /// If one does not exist and the property is a <see cref="SystemFieldAttribute" />
        /// then the property name is used.
        /// If no attributes are present the property is considered a user field and prefixed the standard
        /// 'U_' to the property name.
        /// </remarks>
        private string GetFieldName(PropertyInfo property)
        {
            if (HasAttribute<SystemFieldAttribute>(property) && HasAttribute<FieldNameAttribute>(property))            
                return property.GetCustomAttribute<FieldNameAttribute>().FieldName;

            if (HasAttribute<SystemFieldAttribute>(property) && !HasAttribute<FieldNameAttribute>(property))
                return property.Name;
            
            if (!HasAttribute<SystemFieldAttribute>(property) && HasAttribute<FieldNameAttribute>(property))
                return $@"U_{property.GetCustomAttribute<FieldNameAttribute>().FieldName}";

            if (!HasAttribute<SystemFieldAttribute>(property) && !HasAttribute<FieldNameAttribute>(property))
                return $@"U_{property.Name}";

            // we should never be able to reach this exception
            throw new InvalidOperationException();
        }

        private IEnumerable<PropertyInfo> GetSerializableProperties(Type t)
        {
            return t.GetProperties().Where(p => HasSystemOrUserField(p) && p.CanWrite);
        }

        private IEnumerable<PropertyInfo> GetSystemAndUserFields()
        {
            return typeof(T).GetProperties().Where(HasSystemOrUserField);
        }

        private bool HasSystemOrUserField(PropertyInfo p)
        {
            return HasAttribute<UserFieldAttribute>(p) || HasAttribute<SystemFieldAttribute>(p);
        }

        private IEnumerable<XElement> GetXmlProperties(object instance)
        {
            var type = instance.GetType();
            var properties = GetSerializableProperties(type);

            foreach (var property in properties)
                if (ConvertToString(property, instance) != null)
                    yield return new XElement(GetFieldName(property), ConvertToString(property, instance));
        }

        private bool HasAttribute<TAttr>(PropertyInfo property) where TAttr : Attribute
        {
            return property.GetCustomAttribute<TAttr>() != null;
        }

        private bool IsSystemTimeProperty(PropertyInfo property)
        {
            return HasAttribute<SystemFieldAttribute>(property) &&
                   (@"CreateTime".Equals(property.Name) || @"UpdateTime".Equals(property.Name));
        }

        private static bool IsUserTimeProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<UserFieldAttribute>()?.SubType != BoFldSubTypes.st_Time;
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

        private List<PropertyInfo> ChildSystemAndUserFields(Type childType)
        {
            return childType.GetProperties().Where(HasSystemOrUserField).ToList();
        }

        private void SetProperty(PropertyInfo property, XElement element, object instance)
        {
            string fieldName = GetFieldName(property);
            var fieldNode = element.Element($@"{fieldName}");

            var value = ConvertToObject(fieldNode, property);
            var setter = property.DeclaringType?.GetProperty(property.Name)?.GetSetMethod(true);
            setter?.Invoke(instance, new[] {value});
        }
    }
}
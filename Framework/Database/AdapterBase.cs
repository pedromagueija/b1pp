// <copyright filename="AdapterBase.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// Common code to extract values from XML and fill in the Metadata objects.
    /// </summary>
    internal class AdapterBase
    {
        /// <summary>
        /// Extracts the value from the <paramref name="attribute"/> and<para/>
        /// performs necessary conversions, e.g.: to B1 enumerations, when required.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <returns>
        /// The value for the property.
        /// </returns>
        private object GetPropertyValue(XAttribute attribute, Type propertyType)
        {
            var attributeValue = attribute.Value;
            object value;

            if (propertyType.IsEnum)
            {
                var converter = new B1EnumConverter();
                value = converter.Convert(propertyType, attributeValue);
            }
            else
            {
                value = Convert.ChangeType(attributeValue, propertyType, CultureInfo.InvariantCulture);
            }

            return value;
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributes">The attributes.</param>
        /// <param name="instance">The instance.</param>
        protected void PopulateProperties<T>(IEnumerable<XAttribute> attributes, object instance)
        {
            foreach (var attribute in attributes)
            {
                PopulateProperty<T>(instance, attribute);
            }
        }

        private void PopulateProperty<T>(object instance, XAttribute attribute)
        {
            var propertyName = attribute.Name.LocalName ?? string.Empty;

            try { 
                var property = typeof(T).GetProperty(propertyName);
                var value = GetPropertyValue(attribute, property.PropertyType);

                property.SetValue(instance, value);
            }
            catch (Exception e) when (
               e is ArgumentNullException || 
               e is AmbiguousMatchException || 
               e is TargetException || 
               e is MethodAccessException || 
               e is TargetInvocationException)
            {
                throw new SetPropertyException($"Couldn't set property '{propertyName}'.", e);
            }
        }

        protected void PopulateCollection<T>(IEnumerable<XElement> elements, object instance)
        {
            foreach (var element in elements)
            {
                var attributes = element.Attributes();
                PopulateProperties<T>(attributes, instance);

                typeof(T).GetMethod("Add").Invoke(instance, new object[]{});
            }
        }
    }
}
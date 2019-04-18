// <copyright filename="GetFieldNameFromProperty.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Reflection;
using B1PP.Database;
using B1PP.Database.Attributes;
using B1PP.Extensions.Common;

namespace B1PP
{
    /// <summary>
    /// Gets the field name.
    /// </summary>
    /// <remarks>
    /// The field name is extracted from the <see cref="FieldNameAttribute" /> if one exists.
    /// This allows us to name the properties correctly even when field names are abbreviated.
    /// If one does not exist and the property is a <see cref="SystemFieldAttribute" />
    /// then the property name is used.
    /// If no attributes are present the property is considered a user field and prefixed the standard
    /// 'U_' to the property name.
    /// </remarks>
    internal class GetFieldName
    {
        /// <summary>
        /// Executes the getter.
        /// </summary>
        /// <param name="property">
        /// Property for which we wish to get the field name.
        /// </param>
        /// <returns>
        /// A string with the field name.
        /// </returns>
        public string FromProperty(PropertyInfo property)
        {
            bool isSystemField = property.HasAttribute<SystemFieldAttribute>();
            string fieldName = GetName(property);

            if (isSystemField)
            {
                return fieldName;
            }

            return $@"U_{fieldName}";
        }

        private string GetName(PropertyInfo property)
        {
            if (property.HasAttribute<FieldNameAttribute>())
                return property.GetCustomAttribute<FieldNameAttribute>().FieldName;

            return property.Name;
        }
        
    }
}
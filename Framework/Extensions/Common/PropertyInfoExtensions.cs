// <copyright filename="PropertyInfo.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Reflection;

namespace B1PP.Extensions.Common
{
    internal static class PropertyInfoExtensions
    {
        private static readonly GetFieldName FieldName = new GetFieldName();
        
        public static bool HasAttribute<TAttr>(this PropertyInfo property) where TAttr : Attribute
        {
            return property.GetCustomAttribute<TAttr>() != null;
        }

        public static string GetFieldName(this PropertyInfo property)
        {
            return FieldName.FromProperty(property);
        }
    }
}
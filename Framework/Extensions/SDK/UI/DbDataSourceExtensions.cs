// <copyright filename="DbDataSourceExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Database.Attributes;

    using SAPbouiCOM;

    /// <summary>
    /// Common and helpful methods for <see cref="DBDataSource"/>.
    /// </summary>
    public static class DbDataSourceExtensions
    {
        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this DBDataSource source, string columnId, int rowIndex)
        {
            string sourceValue = source.GetString(columnId, rowIndex);
            if (string.IsNullOrEmpty(sourceValue))
            {
                return null;
            }

            DateTime value = DateTime.ParseExact(sourceValue, "yyyyMMdd", CultureInfo.InvariantCulture);
            return value;
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static double? GetDouble(this DBDataSource source, string columnId, int rowIndex)
        {
            string sourceValue = source.GetString(columnId, rowIndex);
            if (string.IsNullOrEmpty(sourceValue))
            {
                return null;
            }

            double value = double.Parse(sourceValue, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);

            return value;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static int? GetInt(this DBDataSource source, string columnId, int rowIndex)
        {
            string sourceValue = source.GetString(columnId, rowIndex);
            if (string.IsNullOrEmpty(sourceValue))
            {
                return null;
            }

            int value = int.Parse(sourceValue, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);

            return value;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static string GetString(this DBDataSource source, string columnId, int rowIndex)
        {
            string sourceValue = source.GetValue(columnId, rowIndex);

            return sourceValue?.Trim();
        }

        /// <summary>
        /// Sets the specified column identifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        public static void Set(this DBDataSource source, string columnId, int rowIndex, int? value)
        {
            if (value != null)
            {
                source.SetValue(columnId, rowIndex, ((int) value).ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets the specified column identifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        public static void Set(this DBDataSource source, string columnId, int rowIndex, string value)
        {
            if (value != null)
            {
                source.SetValue(columnId, rowIndex, value);
            }
        }

        /// <summary>
        /// Sets the specified column identifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        public static void Set(this DBDataSource source, string columnId, int rowIndex, double? value)
        {
            if (value != null)
            {
                source.SetValue(columnId, rowIndex, ((double) value).ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets the specified column identifier.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        public static void Set(this DBDataSource source, string columnId, int rowIndex, DateTime? value)
        {
            if (value != null)
            {
                source.SetValue(columnId, rowIndex, ((DateTime) value).ToString("yyyyMMdd"));
            }
        }


        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<T> ToList<T>(this DBDataSource source) where T : class, new()
        {
            int rowCount = source.Size;
            if (rowCount <= 0)
                yield break;

            for (int row = 0; row < rowCount; row++)
            {
                T item = CreateNewItem<T>(source, row);
                yield return item;
            }
        }

        /// <summary>
        /// Creates the new item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        private static T CreateNewItem<T>(DBDataSource source, int rowIndex) where T : class, new()
        {
            Fields sourceFields = source.Fields;
            int columnCount = sourceFields.Count;
            T item = new T();
            Type type = typeof(T);

            for (int column = 0; column < columnCount; column++)
            {
                string columnName = sourceFields.Item(column).Name;
                var property = DetermineProperty(type, columnName);

                if (property.PropertyType == typeof(string))
                    property.SetValue(item, source.GetString(columnName, rowIndex));
                if (property.PropertyType == typeof(int))
                    property.SetValue(item, source.GetInt(columnName, rowIndex));
                if (property.PropertyType == typeof(double))
                    property.SetValue(item, source.GetDouble(columnName, rowIndex));
                if (property.PropertyType == typeof(DateTime))
                    property.SetValue(item, source.GetDateTime(columnName, rowIndex));
            }

            return item;
        }

        /// <summary>
        /// Determines the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private static PropertyInfo DetermineProperty(Type type, string columnName)
        {
            string propertyName = DeterminePropertyName(columnName);
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                property = properties.First(UserFieldNameAttributeMatch(propertyName));
            }

            return property;
        }

        /// <summary>
        /// Users the field name attribute match.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        private static Func<PropertyInfo, bool> UserFieldNameAttributeMatch(string columnName)
        {
            return p => columnName.Equals(p.GetCustomAttribute<FieldNameAttribute>()?.FieldName);
        }

        /// <summary>
        /// Determines the name of the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string DeterminePropertyName(string name)
        {
            return name.StartsWith(@"U_") ? name.Substring(2) : name;
        }
    }
}
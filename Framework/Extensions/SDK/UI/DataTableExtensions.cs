// <copyright filename="DataTableExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Common;

    using JetBrains.Annotations;

    using SAPbouiCOM;

    using Types;

    /// <summary>
    /// Common and helpful for <see cref="DataTable"/>.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="value">The value.</param>
        public static void AddValue<T>(this DataTable dataTable, T value)
        {
            AddValue(dataTable, value, null);
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="value">The value.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public static void AddValue<T>(this DataTable dataTable, T value, int? rowIndex)
        {
            if (rowIndex == null && dataTable.Rows.Count == 0)
            {
                dataTable.Rows.Add();
            }

            int lastRowIndex = rowIndex ?? dataTable.Rows.Count - 1;
            PopulateRow(dataTable, value, lastRowIndex);
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="value">The value.</param>
        public static void AppendValue<T>(this DataTable dataTable, T value)
        {
            dataTable.Rows.Add();
            int lastRowIndex = dataTable.Rows.Count - 1;

            PopulateRow(dataTable, value, lastRowIndex);
        }

        /// <summary>
        /// Appends the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="values">The values.</param>
        /// <param name="startFromRowIndex">Start index of from row.</param>
        public static void AppendValues<T>(this DataTable dataTable, IEnumerable<T> values, int startFromRowIndex)
        {
            values = values.ToList();
            if (!values.Any())
            {
                return;
            }

            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            var currentData = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_All));
            int rowIndex = startFromRowIndex + 1;
            var rowsToAdd = CreateDataCells(values, properties);

            var row = currentData.XPathSelectElements($"//Row[position() = {rowIndex}]").ToList();

            row.First().AddAfterSelf(rowsToAdd);
            row.Remove();
            
            dataTable.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, currentData.ToString());
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Cannot be null or empty. - columnId
        /// or
        /// columnId
        /// </exception>
        public static DataColumn GetColumn(this DataTable dataTable, string columnId, int rowIndex)
        {
            if (rowIndex.OutsideRange(0, dataTable.Rows.Count - 1))
            {
                return null;
            }

            if (string.IsNullOrEmpty(columnId))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(columnId));
            }

            var column = dataTable.Columns.Cast<DataColumn>().First(c => columnId.Equals(c.Name));
            if (column == null)
            {
                throw new ArgumentException(
                    $"DataTable '{dataTable.UniqueID}' does not contain a column with id '{columnId}'.",
                    nameof(columnId));
            }

            return column;
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">columnId</exception>
        public static DateTime? GetDateTime(this DataTable dataTable, string columnId, int rowIndex)
        {
            DataColumn column = dataTable.GetColumn(columnId, rowIndex);

            if (column.Type != BoFieldsType.ft_Date)
            {
                throw new ArgumentException(
                    $"Expected '{columnId}' to be of type 'BoFieldsType.ft_Date' but is '{dataTable.Columns.Item(columnId).Type}' instead.",
                    nameof(columnId));
            }

            var data = dataTable.GetValue(columnId, rowIndex);
            var value = Convert.ToDateTime(data);
            return value;
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static double? GetDouble(this DataTable dataTable, string columnId, int rowIndex)
        {
            var data = dataTable.GetValue(columnId, rowIndex);
            double? value = Convert.ToDouble(data);
            return value;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public static int? GetInt(this DataTable dataTable, string columnId, int rowIndex)
        {
            var data = dataTable.GetValue(columnId, rowIndex);
            int? value = Convert.ToInt32(data);
            return value;
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>(this DataTable dataTable) where T : class, new()
        {
            var rowCount = dataTable.Rows.Count;
            if (rowCount <= 0)
            {
                return new List<T>();
            }

            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            var xdata = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_All));

            var values = from dataRow in xdata.Descendants("Row")
                select CreateNewValue<T>(dataRow, properties);

            return values;
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="range">The range.</param>
        public static void RemoveRange(this DataTable dataTable, IEnumerable<int> range)
        {
            var dataRows = dataTable.Rows;

            foreach (var index in range.OrderByDescending(i => i))
            {
                dataRows.Remove(index);
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="values">The values.</param>
        public static void SetValues<T>(this DataTable dataTable, IEnumerable<T> values)
        {
            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            var dataCells = CreateDataCells(values, properties);

            var xdata = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));
            var rows = xdata.XPathSelectElement("//Rows");
            if (rows == null)
                return;

            rows.ReplaceAll(dataCells);
            dataTable.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, xdata.ToString());
        }

        /// <summary>
        /// Changes the type.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [CanBeNull]
        private static object ChangeType(string textValue, Type type)
        {
            if (type == typeof(Id))
            {
                var conv = TypeDescriptor.GetConverter(type);
                return conv.ConvertFrom(textValue);
            }

            if (type == typeof(DateTime))
            {
                return DateTime.ParseExact(textValue, @"yyyyMMdd", CultureInfo.InvariantCulture);
            }

            if (type == typeof(int))
            {
                return int.Parse(textValue, CultureInfo.InvariantCulture);
            }

            if (type == typeof(double))
            {
                return double.Parse(textValue, CultureInfo.InvariantCulture);
            }

            if (type == typeof(bool))
            {
                return bool.Parse(textValue);
            }

            if (type == typeof(string))
            {
                return textValue;
            }

            return null;
        }

        /// <summary>
        /// Determines whether [contains] [the specified columns].
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// <c>true</c> if [contains] [the specified columns]; otherwise, <c>false</c>.
        /// </returns>
        private static bool Contains(DataColumns columns, string name)
        {
            for (var i = 0; i < columns.Count; i++)
            {
                if (columns.Item(i).Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates the data cells.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private static IEnumerable<XElement> CreateDataCells<T>(
            IEnumerable<T> values,
            IEnumerable<PropertyInfo> properties)
        {
            return from value in values
                select new XElement(@"Row",
                    new XElement(@"Cells",
                        from property in properties
                        select new XElement(@"Cell",
                            new XElement(@"ColumnUid", property.Name),
                            new XElement(@"Value", GetValue(property, value)))));
        }

        /// <summary>
        /// Creates the new value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow">The data row.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private static T CreateNewValue<T>(XElement dataRow, List<PropertyInfo> properties) where T : class, new()
        {
            var instance = new T();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var textValue = GetValue(dataRow, propertyName);
                var propertyType = property.PropertyType;
                var value = ChangeType(textValue, propertyType);

                property.SetValue(instance, value);
            }
            return instance;
        }

        /// <summary>
        /// Gets the properties matching columns.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        private static List<PropertyInfo> GetPropertiesMatchingColumns<T>(DataTable dataTable)
        {
            return typeof(T).GetProperties().Where(p => Contains(dataTable.Columns, p.Name)).ToList();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object GetValue<T>(PropertyInfo property, T value)
        {
            var propertyValue = property.GetValue(value);

            if (propertyValue is DateTime dateTime)
            {
                return dateTime.ToString(GlobalConstants.AnsiSqlDateTimeFormat);
            }

            return Convert.ToString(propertyValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private static string GetValue(XElement dataRow, string propertyName)
        {
            var propertyValue = dataRow.XPathSelectElement($"Cells/Cell/Value[../ColumnUid='{propertyName}']").Value;
            return propertyValue;
        }

        /// <summary>
        /// Populates the row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">The data table.</param>
        /// <param name="value">The value.</param>
        /// <param name="rowIndex">Index of the row.</param>
        private static void PopulateRow<T>(DataTable dataTable, T value, int rowIndex)
        {
            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            foreach (var property in properties)
            {
                dataTable.SetValue(property.Name, rowIndex, property.GetValue(value));
            }
        }
    }
}
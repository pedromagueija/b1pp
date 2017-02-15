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

    using SAPbobsCOM;

    using SAPbouiCOM;

    using Types;

    public static class DataTableExtensions
    {
        public static void AddValue<T>(this DataTable dataTable, T value)
        {
            AddValue(dataTable, value, null);
        }

        public static void AddValue<T>(this DataTable dataTable, T value, int? rowIndex)
        {
            if ((rowIndex == null) && (dataTable.Rows.Count == 0))
            {
                dataTable.Rows.Add();
            }

            int lastRowIndex = rowIndex ?? dataTable.Rows.Count - 1;
            PopulateRow(dataTable, value, lastRowIndex);
        }

        public static void AppendValue<T>(this DataTable dataTable, T value)
        {
            dataTable.Rows.Add();
            int lastRowIndex = dataTable.Rows.Count - 1;

            PopulateRow(dataTable, value, lastRowIndex);
        }

        public static void AppendValues<T>(this DataTable dataTable, IEnumerable<T> values, int startFromRowIndex)
        {
            values = values.ToList();
            if (!values.Any())
            {
                return;
            }

            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            XDocument currentData = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_All));
            int row = startFromRowIndex + 1;
            var rowsToAdd = CreateDataCells(values, properties);

            currentData.XPathSelectElements($"//Row[position() = {row}]").First().AddAfterSelf(rowsToAdd);
            currentData.XPathSelectElements($"//Row[position() = {row}]").Remove();
            dataTable.LoadSerializedXML(BoDataTableXmlSelect.dxs_All, currentData.ToString());
        }

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

            DataColumn column = dataTable.Columns.Cast<DataColumn>().First(c => columnId.Equals(c.Name));
            if (column == null)
            {
                throw new ArgumentException(
                    $"DataTable '{dataTable.UniqueID}' does not contain a column with id '{columnId}'.",
                    nameof(columnId));
            }

            return column;
        }

        public static DateTime? GetDateTime(this DataTable dataTable, string columnId, int rowIndex)
        {
            DataColumn column = dataTable.GetColumn(columnId, rowIndex);

            if (column.Type != BoFieldsType.ft_Date)
            {
                throw new ArgumentException(
                    $"Expected '{columnId}' to be of type 'BoFieldsType.ft_Date' but is '{dataTable.Columns.Item(columnId).Type}' instead.",
                    nameof(columnId));
            }

            object data = dataTable.GetValue(columnId, rowIndex);
            DateTime value = Convert.ToDateTime(data);
            return value;
        }

        public static int? GetInt(this DataTable dataTable, string columnId, int rowIndex)
        {
            object data = dataTable.GetValue(columnId, rowIndex);
            int? value = Convert.ToInt32(data);
            return value;
        }

        public static double? GetDouble(this DataTable dataTable, string columnId, int rowIndex)
        {
            object data = dataTable.GetValue(columnId, rowIndex);
            double? value = Convert.ToDouble(data);
            return value;
        }

        public static IEnumerable<T> GetValues<T>(this DataTable dataTable) where T : class, new()
        {
            int rowCount = dataTable.Rows.Count;
            if (rowCount <= 0)
            {
                return new List<T>();
            }

            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            XDocument xdata = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_All));

            var values = from dataRow in xdata.Descendants("Row")
                select CreateNewValue<T>(dataRow, properties);

            return values;
        }

        public static void RemoveRange(this DataTable dataTable, IEnumerable<int> range)
        {
            DataRows dataRows = dataTable.Rows;

            foreach (int index in range.OrderByDescending(i => i))
            {
                dataRows.Remove(index);
            }
        }

        public static void SetValues<T>(this DataTable dataTable, IEnumerable<T> values)
        {
            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            var dataCells = CreateDataCells(values, properties);

            XDocument xdata = XDocument.Parse(dataTable.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly));
            xdata.XPathSelectElement("//Rows").ReplaceAll(dataCells);
            dataTable.LoadSerializedXML(BoDataTableXmlSelect.dxs_DataOnly, xdata.ToString());
        }

        private static bool Contains(DataColumns columns, string name)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns.Item(i).Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<XElement> CreateDataCells<T>(
            IEnumerable<T> values,
            IEnumerable<PropertyInfo> properties)
        {
            return from value in values
                select new XElement("Row",
                    new XElement("Cells",
                        from property in properties
                        select new XElement("Cell",
                            new XElement("ColumnUid", property.Name),
                            new XElement("Value", GetValue(property, value)))));
        }

        private static T CreateNewValue<T>(XElement dataRow, List<PropertyInfo> properties) where T : class, new()
        {
            var instance = new T();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string textValue = GetValue(dataRow, propertyName);
                Type propertyType = property.PropertyType;
                object value = ChangeType(textValue, propertyType);

                property.SetValue(instance, value);
            }
            return instance;
        }

        [CanBeNull]
        private static object ChangeType(string textValue, Type type)
        {
            if (type == typeof(Id))
            {
                TypeConverter conv = TypeDescriptor.GetConverter(type);
                return conv.ConvertFrom(textValue);
            }

            if (type == typeof(DateTime))
            {
                return DateTime.ParseExact(textValue, "yyyyMMdd", CultureInfo.InvariantCulture);
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

        private static List<PropertyInfo> GetPropertiesMatchingColumns<T>(DataTable dataTable)
        {
            return typeof(T).GetProperties().Where(p => Contains(dataTable.Columns, p.Name)).ToList();
        }

        private static object GetValue<T>(PropertyInfo property, T value)
        {
            object propertyValue = property.GetValue(value);

            if (propertyValue is DateTime)
            {
                var dateTime = (DateTime) propertyValue;
                return dateTime.ToString("yyyyMMdd");
            }

            return Convert.ToString(propertyValue, CultureInfo.InvariantCulture);
        }

        private static string GetValue(XElement dataRow, string propertyName)
        {
            string propertyValue = dataRow.XPathSelectElement($"Cells/Cell/Value[../ColumnUid='{propertyName}']").Value;
            return propertyValue;
        }

        private static void PopulateRow<T>(DataTable dataTable, T value, int rowIndex)
        {
            var properties = GetPropertiesMatchingColumns<T>(dataTable);
            foreach (PropertyInfo property in properties)
            {
                dataTable.SetValue(property.Name, rowIndex, property.GetValue(value));
            }
        }
    }
}
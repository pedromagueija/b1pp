// <copyright filename="XmlRecordsetReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    using SAPbobsCOM;

    /// <summary>
    /// Allows you to read the data in the recordset.
    /// </summary>
    internal sealed class XmlRecordsetReader : IRecordsetReader
    {
        private const string SapNamespace = "xmlns=\"http://www.sap.com/SBO/SDK/DI\"";

        private List<Column> columns = new List<Column>();

        private XmlNode currentRow;

        private IEnumerator rowEnumerator;

        public IEnumerable<IColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        private XmlRecordsetReader()
        {
            
        }

        public DateTime? GetDateTime(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                return DateTime.ParseExact(value, GlobalConstants.BusinessOneDateTimeFormat, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw CreateFormatException(value, typeof(DateTime));
            }
        }

        public DateTime GetDateTimeOrDefault(string columnName)
        {
            return GetDateTime(columnName) ?? default;
        }

        public double? GetDouble(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            bool success = double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double returnValue);
            if (success)
            {
                return returnValue;
            }

            throw CreateFormatException(value, typeof(double));
        }

        public double GetDoubleOrDefault(string columnName)
        {
            return GetDouble(columnName) ?? default;
        }

        private FormatException CreateFormatException(string value, Type target)
        {
            string message = $@"String value {value} cannot be parsed into an {target} value.";
            return new FormatException(message);
        }

        public int? GetInt(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            bool success = int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int returnValue);
            if (success)
            {
                return returnValue;
            }

            throw CreateFormatException(value, typeof(int));
        }

        public int GetIntOrDefault(string columnName)
        {
            return GetInt(columnName) ?? default;
        }

        public bool GetBoolOrDefault(string columnName)
        {
            return GetBool(columnName) ?? default;
        }

        public bool? GetBool(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            bool success = bool.TryParse(value, out bool returnValue);
            if (success)
            {
                return returnValue;
            }

            throw CreateFormatException(value, typeof(bool));
        }

        public string GetString(string columnName)
        {
            return GetValue(columnName);
        }

        public string GetStringOrEmpty(string columnName)
        {
            return GetString(columnName) ?? string.Empty;
        }

        public bool MoveNext()
        {
            if (rowEnumerator == null)
            {
                return false;
            }

            bool retVal = rowEnumerator.MoveNext();
            currentRow = (XmlNode) rowEnumerator.Current;
            return retVal;
        }

        public static XmlRecordsetReader CreateNew(Recordset data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var reader = new XmlRecordsetReader();
            reader.Load(data);

            return reader;
        }

        private XmlDocument ToXmlDocument(Recordset data)
        {
            string fixedXml = data.GetFixedXML(RecordsetXMLModeEnum.rxmData);
            if (string.IsNullOrEmpty(fixedXml))
            {
                return null;
            }

            string xmlData = fixedXml.Replace(SapNamespace, string.Empty);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            return xmlDocument;
        }

        private string GetValue(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException($@"Parameter {nameof(columnName)} cannot be null or empty.");
            }

            try
            {
                string xpath = $@"Fields/Field/Value[../Alias = '{columnName}']";
                var valueNode = currentRow.SelectSingleNode(xpath);
                if (valueNode != null)
                {
                    return valueNode.InnerText;
                }

                throw new ArgumentException($@"No column with name {columnName} was found.");
            }
            catch (XPathException)
            {
                // TODO: log the exception
                return string.Empty;
            }
        }

        private void Load(Recordset data)
        {
            var xmlDoc = ToXmlDocument(data);

            var rows = xmlDoc?.SelectNodes(@"//Row");
            if (rows == null || rows.Count == 0)
            {
                return;
            }

            rowEnumerator = rows.GetEnumerator();

            var firstNode = rows[0];
            var aliases = firstNode.SelectNodes(@"descendant::Alias");            
            if (aliases == null)
            {
                return;
            }

            var fields = data.Fields;
            foreach (XmlNode alias in aliases)
            {
                string name = alias.InnerXml;
                var type = fields.Item(name).Type;

                columns.Add(new Column(name, type));
            }

            // TODO: why is this here?
            columns = columns.ToList();
        }
    }
}
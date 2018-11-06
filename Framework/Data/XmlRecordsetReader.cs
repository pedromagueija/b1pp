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
    internal class XmlRecordsetReader : IRecordsetReader
    {
        private const string SapNamespace = "xmlns=\"http://www.sap.com/SBO/SDK/DI\"";

        private readonly XmlDocument xmlDoc;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRecordsetReader" /> class.
        /// </summary>
        private XmlRecordsetReader()
        {
            xmlDoc = new XmlDocument();
            rowEnumerator = xmlDoc.GetEnumerator();
        }

        public DateTime? GetDateTime(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.ParseExact(value, GlobalConstants.BusinessOneDateTimeFormat, CultureInfo.InvariantCulture);
        }

        public DateTime GetDateTimeOrDefault(string columnName)
        {
            return GetDateTime(columnName) ?? default(DateTime);
        }

        public double? GetDouble(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        public double GetDoubleOrDefault(string columnName)
        {
            return GetDouble(columnName) ?? default(double);
        }

        public int? GetInt(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        public int GetIntOrDefault(string columnName)
        {
            return GetInt(columnName) ?? default(int);
        }

        public bool GetBoolOrDefault(string columnName)
        {
            return GetBool(columnName) ?? default(bool);
        }

        public bool? GetBool(string columnName)
        {
            string value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return bool.Parse(value);
        }

        public string GetString(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException($"Parameter {nameof(columnName)} cannot be null or empty");
            }

            var valueNode = GetItemXmlNode(columnName);

            if (valueNode != null)
            {
                return valueNode.InnerText;
            }

            throw new ArgumentException($"No column with name {columnName} was found.");
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

        public static XmlRecordsetReader CreateNew(IRecordset data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var reader = new XmlRecordsetReader();
            reader.Load(data);

            return reader;
        }

        private string ExtractXmlFromRecordset(IRecordset data)
        {
            string fixedXml = data.GetFixedXML(RecordsetXMLModeEnum.rxmData);
            if (string.IsNullOrEmpty(fixedXml))
            {
                return string.Empty;
            }

            return fixedXml.Replace(SapNamespace, string.Empty);
        }

        private XmlNode GetItemXmlNode(string columnName)
        {
            XmlNode valueNode;

            try
            {
                string xpath = $"Fields/Field/Value[../Alias = '{columnName}']";
                valueNode = currentRow.SelectSingleNode(xpath);
            }
            catch (XPathException)
            {
                valueNode = null;
            }

            return valueNode;
        }

        private void Load(IRecordset data)
        {
            // get data
            string xmlData = ExtractXmlFromRecordset(data);

            if (string.IsNullOrEmpty(xmlData))
            {
                return;
            }

            xmlDoc.LoadXml(xmlData);

            var rows = xmlDoc.SelectNodes(@"//Row");
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
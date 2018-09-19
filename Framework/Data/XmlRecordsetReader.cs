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
        /// <summary>
        /// The sap namespace.
        /// </summary>
        private const string SapNamespace = "xmlns=\"http://www.sap.com/SBO/SDK/DI\"";

        /// <summary>
        /// The xml doc.
        /// </summary>
        private readonly XmlDocument xmlDoc;

        /// <summary>
        /// The columns names.
        /// </summary>
        private List<Column> columns = new List<Column>();

        /// <summary>
        /// The current row node.
        /// </summary>
        private XmlNode currentRow;

        /// <summary>
        /// The enumerator.
        /// </summary>
        private IEnumerator rowEnumerator;

        /// <summary>
        /// Returns the columns names.
        /// </summary>
        /// <value>
        /// The columns names.
        /// </value>
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

        /// <summary>
        /// Gets a date time from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime" /> value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist.
        /// </exception>
        /// <exception cref="FormatException">
        /// Triggered when the value contained in <paramref name="columnName" /> is empty or not a date.
        /// </exception>
        public DateTime? GetDateTime(string columnName)
        {
            var value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.ParseExact(value, @"yyyyMMdd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a double from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The double value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The value contained in <paramref name="columnName" /> represents a number that is less than
        /// <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// The value contained in <paramref name="columnName" /> does not represent a number in a
        /// valid format.
        /// </exception>
        public double? GetDouble(string columnName)
        {
            var value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets an integer from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The integer value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value contained in <paramref name="columnName" /> represents a number less than
        /// <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        /// <exception cref="FormatException">The value contained in <paramref name="columnName" /> is not of the correct format. </exception>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        public int? GetInt(string columnName)
        {
            var value = GetString(columnName);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a boolean value from the reader.
        /// </summary>
        /// <param name="columnName">The item identifier.</param>
        /// <returns>The boolean value.</returns>
        /// <exception cref="ArgumentException">Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.</exception>
        /// <exception cref="FormatException">Thrown when column value is not True or False literals.</exception>
        public bool GetBool(string columnName)
        {
            var value = GetString(columnName);
            return bool.Parse(value);
        }

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// Value as a string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
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

        /// <summary>
        /// Moves to the next record.
        /// </summary>
        /// <returns>
        /// True if there is another record, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (rowEnumerator != null)
            {
                var retVal = rowEnumerator.MoveNext();
                currentRow = (XmlNode) rowEnumerator.Current;
                return retVal;
            }

            return false;
        }

        /// <summary>
        /// Creates a new <see cref="XmlRecordsetReader" /> to read the given <paramref name="data" />.
        /// </summary>
        /// <param name="data">The recordset to be read.</param>
        /// <returns>
        /// The record set reader.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data" /> is <see langword="null" />.</exception>
        /// <exception cref="XPathException">The XPath expression contains a prefix. </exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
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

        /// <summary>
        /// Extracts the XML from recordset.
        /// </summary>
        /// <param name="data">The record set with the data.</param>
        /// <returns>
        /// A string that represents the data.
        /// </returns>
        private string ExtractXmlFromRecordset(IRecordset data)
        {
            var fixedXml = data.GetFixedXML(RecordsetXMLModeEnum.rxmData);
            if (string.IsNullOrEmpty(fixedXml))
            {
                return string.Empty;
            }

            return fixedXml.Replace(SapNamespace, string.Empty);
        }

        /// <summary>
        /// Gets the item XML node.
        /// </summary>
        /// <param name="columnName">The item identifier.</param>
        /// <returns>The XML node if it exists, or <see langword="null" /> if it doesn't.</returns>
        private XmlNode GetItemXmlNode(string columnName)
        {
            XmlNode valueNode;

            try
            {
                var xpath = $"Fields/Field/Value[../Alias = '{columnName}']";
                valueNode = currentRow.SelectSingleNode(xpath);
            }
            catch (XPathException)
            {
                valueNode = null;
            }

            return valueNode;
        }

        /// <summary>
        /// Loads the record set data.
        /// </summary>
        /// <param name="data">
        /// The record set with the data.
        /// </param>
        /// <exception cref="XmlException">
        /// There is a load or parse error in the XML.
        /// <para />
        /// In this case, the document remains empty.
        /// </exception>
        /// <exception cref="XPathException">
        /// The XPath expression contains a prefix.
        /// </exception>
        private void Load(IRecordset data)
        {
            // get data
            var xmlData = ExtractXmlFromRecordset(data);

            if (string.IsNullOrEmpty(xmlData))
            {
                return;
            }

            xmlDoc.LoadXml(xmlData);

            var rows = xmlDoc.SelectNodes(@"//Row");
            if (rows != null)
            {
                rowEnumerator = rows.GetEnumerator();

                var firstNode = rows[0];
                var aliases = firstNode.SelectNodes(@"descendant::Alias");
                var fields = data.Fields;
                if (aliases != null)
                {
                    foreach (XmlNode alias in aliases)
                    {
                        var name = alias.InnerXml;
                        var type = fields.Item(name).Type;

                        columns.Add(new Column(name, type));
                    }

                    // TODO: why is this here?
                    columns = columns.ToList();
                }
            }
        }
    }
}
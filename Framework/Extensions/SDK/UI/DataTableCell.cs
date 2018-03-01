// <copyright filename="DataTableCell.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    internal class DataTableCell : IXmlSerializable
    {
        private string ColumnUid { get; set; }

        private string Value { get; set; }

        public DataTableCell(string columnId, string value)
        {
            Ensure.NotNullOrEmpty(nameof(columnId), columnId);

            ColumnUid = columnId;
            Value = value;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            ColumnUid = reader.ReadElementContentAsString(nameof(ColumnUid), string.Empty);
            Value = reader.ReadElementContentAsString(nameof(Value), string.Empty);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(@"Cell");
            writer.WriteElementString(nameof(ColumnUid), string.Empty, ColumnUid);
            writer.WriteElementString(nameof(Value), string.Empty, Value);
            writer.WriteEndElement();
        }
    }
}
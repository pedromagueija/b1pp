namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks />
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sap.com/SBO/SDK/DI")]
    [XmlRoot("Recordset", Namespace = "http://www.sap.com/SBO/SDK/DI", IsNullable = false)]
    public sealed class RecordsetResult
    {

        /// <remarks />
        [XmlArrayItem("Row", IsNullable = false)]
        public List<RecordsetRow> Rows { get; set; } = new List<RecordsetRow>();

        /// <remarks />
        public string Table { get; set; }
    }
}

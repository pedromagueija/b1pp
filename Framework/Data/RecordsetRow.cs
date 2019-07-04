// <copyright filename="RecordsetRow.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <remarks />
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sap.com/SBO/SDK/DI")]
    public sealed class RecordsetRow
    {
        /// <remarks />
        [XmlArrayItem("Field", IsNullable = false)]
        public List<RecordsetRowField> Fields { get; set; } = new List<RecordsetRowField>();
    }
}
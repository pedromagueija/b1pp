// <copyright filename="RecordsetRowField.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <remarks />
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sap.com/SBO/SDK/DI")]
    public sealed class RecordsetRowField
    {

        /// <remarks />
        public string Alias { get; set; }

        /// <remarks />
        public string Value { get; set; }
    }
}
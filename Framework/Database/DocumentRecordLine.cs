// <copyright filename="DocumentRecordLine.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using Attributes;

    /// <summary>
    /// Represents a line on a user-defined object of type document.
    /// </summary>
    public abstract class DocumentRecordLine
    {
        /// <summary>
        /// Gets or sets the document entry.
        /// </summary>
        /// <value>
        /// The document entry.
        /// </value>
        [SystemField]
        public int? DocEntry { get; protected set; }

        /// <summary>
        /// Gets or sets the line identifier.
        /// </summary>
        /// <value>
        /// The line identifier.
        /// </value>
        [SystemField]
        public int? LineId { get; protected set; }

        /// <summary>
        /// Gets or sets the visual order.
        /// </summary>
        /// <value>
        /// The visual order.
        /// </value>
        [SystemField]
        [FieldName(@"VisOrder")]
        public int? VisualOrder { get; protected set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [SystemField]
        public string Object { get; protected set; }

        /// <summary>
        /// Gets or sets the log instance.
        /// </summary>
        /// <value>
        /// The log instance.
        /// </value>
        [SystemField]
        [FieldName(@"LogInst")]
        public int? LogInstance { get; protected set; }
    }
}
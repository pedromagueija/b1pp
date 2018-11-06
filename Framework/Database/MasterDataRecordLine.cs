// <copyright filename="MasterDataRecordLine.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using Attributes;

    /// <summary>
    /// Represents a line on a user-defined object of type master data.
    /// </summary>
    public abstract class MasterDataRecordLine
    {
        /// <summary>
        /// Gets or sets code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [SystemField]
        public string Code { get; protected set; }

        /// <summary>
        /// Gets or sets the line identifier.
        /// </summary>
        /// <value>
        /// The line identifier.
        /// </value>
        [SystemField]
        public int? LineId { get; protected set; }

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
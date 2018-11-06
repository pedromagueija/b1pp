// <copyright filename="MasterDataRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Local
// Reason: The private setters are essential for setting the properties via reflection.

namespace B1PP.Database
{
    using System;

    using Attributes;

    /// <summary>
    /// Standard Business One data for master data type records.
    /// </summary>
    public abstract class MasterDataRecord : IMasterDataRecord
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [SystemField]
        public string Code { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [SystemField]
        public string Name { get; set; }

        /// <summary>
        /// Gets the master data entry.
        /// </summary>
        /// <value>
        /// The master data entry.
        /// </value>
        [SystemField]
        public int? DocEntry { get; private set; }

        /// <summary>
        /// Gets the canceled.
        /// </summary>
        /// <value>
        /// The canceled.
        /// </value>
        [SystemField]
        [FieldName(@"Canceled")]
        public string Cancelled { get; private set; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [SystemField]
        public string Object { get; private set; }

        /// <summary>
        /// Gets the log instance.
        /// </summary>
        /// <value>
        /// The log instance.
        /// </value>
        [SystemField]
        [FieldName(@"LogInst")]
        public int? LogInstance { get; private set; }

        /// <summary>
        /// Gets the user signature.
        /// </summary>
        /// <value>
        /// The user signature.
        /// </value>
        [SystemField]
        [FieldName(@"UserSign")]
        public int? UserSignature { get; private set; }

        /// <summary>
        /// Gets the transferred.
        /// </summary>
        /// <value>
        /// The transferred.
        /// </value>
        [SystemField]
        [FieldName(@"Transfered")]
        public string Transferred { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [SystemField]
        public string Status { get; private set; }

        /// <summary>
        /// Gets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        [SystemField]
        public DateTime? CreateDate { get; private set; }

        /// <summary>
        /// Gets the create time.
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        [SystemField]
        public int? CreateTime { get; private set; }

        /// <summary>
        /// Gets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        [SystemField]
        public DateTime? UpdateDate { get; private set; }

        /// <summary>
        /// Gets the update time.
        /// </summary>
        /// <value>
        /// The update time.
        /// </value>
        [SystemField]
        public int? UpdateTime { get; private set; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [SystemField]
        public string DataSource { get; private set; }
    }
}
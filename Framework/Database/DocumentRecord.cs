// <copyright filename="DocumentRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;

    using Attributes;

    /// <summary>
    /// Standard Business One data for document type records.
    /// </summary>
    public abstract class DocumentRecord
    {
        /// <summary>
        /// Gets the document entry.
        /// </summary>
        /// <value>
        /// The document entry.
        /// </value>
        [SystemField]
        public int? DocEntry { get; }

        /// <summary>
        /// Gets the document number.
        /// </summary>
        /// <value>
        /// The document number.
        /// </value>
        [SystemField]
        public string DocNum { get; }

        /// <summary>
        /// Gets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        [SystemField]
        public int? Period { get; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        [SystemField]
        public int? Instance { get; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        [SystemField]
        public int? Series { get; }

        /// <summary>
        /// Gets the handwritten.
        /// </summary>
        /// <value>
        /// The handwritten.
        /// </value>
        [SystemField]
        [FieldName(@"Handwrtten")]
        public string Handwritten { get; }

        /// <summary>
        /// Gets the canceled.
        /// </summary>
        /// <value>
        /// The canceled.
        /// </value>
        [SystemField]
        public string Canceled { get; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [SystemField]
        public string Object { get; }

        /// <summary>
        /// Gets the log instance.
        /// </summary>
        /// <value>
        /// The log instance.
        /// </value>
        [SystemField]
        [FieldName(@"LogInst")]
        public int? LogInstance { get; }

        /// <summary>
        /// Gets the user signature.
        /// </summary>
        /// <value>
        /// The user signature.
        /// </value>
        [SystemField]
        [FieldName(@"UserSign")]
        public int? UserSignature { get; }

        /// <summary>
        /// Gets the transfered.
        /// </summary>
        /// <value>
        /// The transfered.
        /// </value>
        [SystemField]
        public string Transfered { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [SystemField]
        public string Status { get; }

        /// <summary>
        /// Gets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        [SystemField]
        public DateTime? CreateDate { get; }

        /// <summary>
        /// Gets the create time.
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        [SystemField]
        public int? CreateTime { get; }

        /// <summary>
        /// Gets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        [SystemField]
        public DateTime? UpdateDate { get; }

        /// <summary>
        /// Gets the update time.
        /// </summary>
        /// <value>
        /// The update time.
        /// </value>
        [SystemField]
        public int? UpdateTime { get; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [SystemField]
        public string DataSource { get; }

        /// <summary>
        /// Gets the request status.
        /// </summary>
        /// <value>
        /// The request status.
        /// </value>
        [SystemField]
        public string RequestStatus { get; }

        /// <summary>
        /// Gets the creator.
        /// </summary>
        /// <value>
        /// The creator.
        /// </value>
        [SystemField]
        public string Creator { get; }

        /// <summary>
        /// Gets the remark.
        /// </summary>
        /// <value>
        /// The remark.
        /// </value>
        [SystemField]
        public string Remark { get; }
    }
}
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
        [SystemField]
        public int? DocEntry { get; }

        [SystemField]
        public string DocNum { get; }

        [SystemField]
        public int? Period { get; }

        [SystemField]
        public int? Instance { get; }

        [SystemField]
        public int? Series { get; }

        [SystemField]
        [FieldName(@"Handwrtten")]
        public string Handwritten { get; }

        [SystemField]
        public string Canceled { get; }

        [SystemField]
        public string Object { get; }

        [SystemField]
        [FieldName(@"LogInst")]
        public int? LogInstance { get; }

        [SystemField]
        [FieldName(@"UserSign")]
        public int? UserSignature { get; }

        [SystemField]
        public string Transfered { get; }

        [SystemField]
        public string Status { get; }

        [SystemField]
        public DateTime? CreateDate { get; }

        [SystemField]
        public int? CreateTime { get; }

        [SystemField]
        public DateTime? UpdateDate { get; }

        [SystemField]
        public int? UpdateTime { get; }

        [SystemField]
        public string DataSource { get; }

        [SystemField]
        public string RequestStatus { get; }

        [SystemField]
        public string Creator { get; }

        [SystemField]
        public string Remark { get; }
    }
}
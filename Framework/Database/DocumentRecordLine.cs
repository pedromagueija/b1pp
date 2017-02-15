// <copyright filename="DocumentRecordLine.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using Attributes;

    public abstract class DocumentRecordLine
    {
        [SystemField]
        public int? DocEntry { get; protected set; }

        [SystemField]
        public int? LineId { get; protected set; }

        [SystemField]
        [FieldName(@"VisOrder")]
        public int? VisualOrder { get; protected set; }

        [SystemField]
        public string Object { get; protected set; }

        [SystemField]
        [FieldName(@"LogInst")]
        public int? LogInstance { get; protected set; }
    }
}
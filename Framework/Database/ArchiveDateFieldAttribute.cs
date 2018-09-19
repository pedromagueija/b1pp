// <copyright filename="ArchiveDateFieldAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System;

    using SAPbobsCOM;

    [AttributeUsage(AttributeTargets.Class)]
    internal class ArchiveDateFieldAttribute : Attribute
    {
        public string FieldName { get; }

        public ArchiveDateFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public void Apply(UserTablesMD table)
        {
            table.Archivable = BoYesNoEnum.tYES;
            table.ArchiveDateField = FieldName;
        }
    }
}
// <copyright filename="ArchiveDateFieldAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using SAPbobsCOM;

namespace B1PP.Database.Attributes
{
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
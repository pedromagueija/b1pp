// <copyright filename="UserTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UserTableAttribute : Attribute
    {
        public BoUTBTableType TableType { get; }
        public string TableName { get; }
        public string TableDescription { get; }

        public UserTableAttribute(
            string tableName,
            string tableDescription,
            BoUTBTableType tableType)
        {
            TableType = tableType;
            TableName = tableName ?? string.Empty;
            TableDescription = tableDescription ?? string.Empty;
        }

        internal void Apply(UserTablesMD table)
        {
            table.TableName = TableName;
            table.TableDescription = TableDescription;
            table.TableType = TableType;
        }
    }
}
// <copyright filename="UserTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    [AttributeUsage(AttributeTargets.Class)]
    public class UserTableAttribute : Attribute
    {
        public BoUTBTableType TableType { get; }
        public string TableName { get; }
        public string TableDescription { get; }

        /// <summary>
        /// Gets the table prefix.
        /// </summary>
        /// <value>
        /// The table prefix.
        /// </value>
        public string TablePrefix { get; }

        public UserTableAttribute(string tableName, string tableDescription, BoUTBTableType tableType,
            string tablePrefix)
        {
            TableType = tableType;
            TablePrefix = tablePrefix ?? string.Empty;
            TableName = tableName ?? string.Empty;
            TableDescription = tableDescription ?? string.Empty;
        }

        public UserTableAttribute(BoUTBTableType tableType, string tablePrefix) :
            this(string.Empty, string.Empty, tableType, tablePrefix)
        {
        }

        internal void Apply(Type type, UserTablesMD table)
        {
            table.TableName = DetermineTableName(type);
            table.TableDescription = DetermineTableDescription(type);
            table.TableType = TableType;
        }

        private string DetermineTableDescription(Type type)
        {
            if (string.IsNullOrWhiteSpace(TableDescription))
            {
                return Utilities.SplitByCaps(type.Name);
            }

            return TableDescription;
        }

        private string DetermineTableName(Type type)
        {
            if (string.IsNullOrWhiteSpace(TableName))
            {
                return $"{TablePrefix}{type.Name}";
            }

            return $"{TablePrefix}{TableName}";
        }
    }
}
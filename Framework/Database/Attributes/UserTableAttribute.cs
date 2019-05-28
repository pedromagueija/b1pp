// <copyright filename="UserTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Mark a class to be created as a user defined table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UserTableAttribute : Attribute
    {
        /// <summary>
        /// Returns the table type.
        /// </summary>
        public BoUTBTableType TableType { get; }
        
        /// <summary>
        /// Returns the table name.
        /// </summary>
        public string TableName { get; }
        
        /// <summary>
        /// Returns the table description.
        /// </summary>
        public string TableDescription { get; }

        
        /// <summary>
        /// Creates a new instance of <see cref="UserTableAttribute"/>.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="tableDescription">The table description.</param>
        /// <param name="tableType">The table type.</param>
        public UserTableAttribute(
            string tableName,
            string tableDescription,
            BoUTBTableType tableType)
        {
            TableType = tableType;
            TableName = tableName ?? string.Empty;
            TableDescription = tableDescription ?? string.Empty;
        }

        // (COM object, not recommended to use the interface IUserTableMD)
        // ReSharper disable once S3242
        internal void Apply(UserTablesMD table)
        {
            table.TableName = TableName;
            table.TableDescription = TableDescription;
            table.TableType = TableType;
        }
    }
}
// <copyright filename="ChildUserTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Marks a class as being a child user table.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ChildUserTableAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the child table.
        /// </summary>
        /// <value>
        /// The name of the child table.
        /// </value>
        public string TableName { get; }

        public string TableDescription { get; }

        public BoUTBTableType TableType { get; }

        /// <summary>
        /// Gets the name of the child object.
        /// </summary>
        /// <value>
        /// The name of the child object.
        /// </value>
        public string ObjectName { get; }

        /// <summary>
        /// Gets the name of the child log table.
        /// </summary>
        /// <value>
        /// The name of the child log table.
        /// </value>
        public string LogTableName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildUserTableAttribute" /> class.
        /// </summary>
        /// <param name="tableName">Name of the child table.</param>
        /// <param name="tableDescription">Description of the child table.</param>
        /// <param name="tableType">Type of the child table.</param>
        /// <param name="objectName">Name of the child object.</param>
        /// <param name="logTableName">Name of the child log table.</param>
        public ChildUserTableAttribute(string tableName, string tableDescription, BoUTBTableType tableType, string objectName, string logTableName)
        {
            TableName = tableName;
            TableDescription = tableDescription;
            ObjectName = objectName;
            LogTableName = logTableName;
            TableType = tableType;
        }

        public ChildUserTableAttribute(string tableName, string tableDescription, BoUTBTableType tableType) :
            this(tableName, tableDescription, tableType, $"{tableName}_O", $"{tableName}_A")
        {
            
        }

        public void Apply(UserTablesMD table)
        {
            table.TableName = TableName;
            table.TableDescription = TableDescription;
            table.TableType = TableType;
        }
    }
}
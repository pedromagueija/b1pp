// <copyright filename="ChildUserTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database.Attributes
{
    using System;

    /// <summary>
    /// Marks a member as being a relationship to a child table.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ChildUserTableAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the child table.
        /// </summary>
        /// <value>
        /// The name of the child table.
        /// </value>
        public string TableName { get; }

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
        /// Initializes a new instance of the <see cref="ChildUserTableAttribute"/> class.
        /// </summary>
        /// <param name="tableName">Name of the child table.</param>
        /// <param name="objectName">Name of the child object.</param>
        /// <param name="logTableName">Name of the child log table.</param>
        public ChildUserTableAttribute(string tableName, string objectName, string logTableName)
        {
            TableName = tableName;
            ObjectName = objectName;
            LogTableName = logTableName;
        }
    }
}
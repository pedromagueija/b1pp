// <copyright filename="SystemTableAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    internal class SystemTableAttribute : Attribute
    {
        public string TableName { get; }

        public SystemTableAttribute(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException(@"Cannot be null or empty.", nameof(tableName));
            }

            TableName = tableName;
        }
    }
}
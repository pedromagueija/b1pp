// <copyright filename="Column.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using SAPbobsCOM;

    /// <summary>
    /// Stores information about a column.
    /// </summary>
    /// <seealso cref="B1PP.Data.IColumn" />
    internal class Column : IColumn
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        /// <value>
        /// The column name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the column type.
        /// </summary>
        /// <value>
        /// The column type.
        /// </value>
        public BoFieldTypes Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="type">The column type.</param>
        public Column(string name, BoFieldTypes type)
        {
            Name = name;
            Type = type;
        }
    }
}
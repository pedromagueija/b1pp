// <copyright filename="IColumn.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using SAPbobsCOM;

    /// <summary>
    /// Stores information about a column.
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        /// <value>
        /// The column name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the column type.
        /// </summary>
        /// <value>
        /// The column type.
        /// </value>
        BoFieldTypes Type { get; }
    }
}
// <copyright filename="MatrixExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.SDK.UI
{
    using System;

    using SAPbouiCOM;

    /// <summary>
    /// Helpful methods for the <see cref="SAPbouiCOM.Matrix"/> object.
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// Gets the index of the column.
        /// This method is chatty.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>
        /// The column index.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Throw when <paramref name="columnId"/> is empty, null or doesn't exist in the matrix.
        /// </exception>
        public static int GetColumnIndex(this Matrix matrix, string columnId)
        {
            if (!string.IsNullOrEmpty(columnId))
            {
                Columns matrixColumns = matrix.Columns;
                int columnCount = matrixColumns.Count;

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    Column column = matrixColumns.Item(columnIndex);
                    if (column.UniqueID == columnId)
                    {
                        return columnIndex;
                    }
                }
            }

            string message = $@"Column '{columnId}' does not exist in matrix '{matrix.Item.UniqueID}'.";
            throw new ArgumentException(message);
        }
    }
}
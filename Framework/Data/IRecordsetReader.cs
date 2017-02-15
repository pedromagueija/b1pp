// <copyright filename="IRecordsetReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// Allows reading of a recordset object contents.
    /// </summary>
    public interface IRecordsetReader
    {
        /// <summary>
        /// Gets a boolean value from the reader.
        /// </summary>
        /// <param name="columnName">The item identifier.</param>
        /// <returns>The boolean value.</returns>
        /// <exception cref="ArgumentException">Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.</exception>
        /// <exception cref="FormatException">Thrown when column value is not True or False literals.</exception>
        bool GetBool(string columnName);

        /// <summary>
        /// Gets a date time from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime" /> value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        /// <exception cref="FormatException">
        /// Triggered when the value contained in <paramref name="columnName" /> is empty or not a date.
        /// </exception>
        DateTime? GetDateTime([NotNull] string columnName);

        /// <summary>
        /// Gets a double from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The double value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The value contained in <paramref name="columnName" /> represents a number that is less than
        /// <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// The value contained in <paramref name="columnName" /> does not represent a number in a
        /// valid format.
        /// </exception>
        double? GetDouble([NotNull] string columnName);

        /// <summary>
        /// Gets an integer from the reader.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// The integer value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value contained in <paramref name="columnName" /> represents a number less than
        /// <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// The value contained in <paramref name="columnName" /> is not of the correct format.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        int? GetInt([NotNull] string columnName);

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        /// <param name="columnName">
        /// The item id.
        /// </param>
        /// <returns>
        /// Value as a string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Triggered when <paramref name="columnName" /> doesn't exist, is null or is empty.
        /// </exception>
        [NotNull]
        string GetString([NotNull] string columnName);

        /// <summary>
        /// Moves to the next record.
        /// </summary>
        /// <returns>
        /// True if there is another record, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The collection was modified after the enumerator was created.
        /// </exception>
        bool MoveNext();

        /// <summary>
        /// Returns the columns names.
        /// </summary>
        /// <value>
        /// The columns names.
        /// </value>
        IEnumerable<IColumn> Columns { get; }
    }
}
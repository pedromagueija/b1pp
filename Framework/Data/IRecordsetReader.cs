// <copyright filename="IRecordsetReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows reading of a recordset object contents.
    /// </summary>
    public interface IRecordsetReader
    {
        /// <summary>
        /// Returns the columns.
        /// </summary>
        IEnumerable<IColumn> Columns { get; }

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        bool GetBoolOrDefault(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        bool? GetBool(string columnName);
        
        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        DateTime? GetDateTime(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        DateTime GetDateTimeOrDefault(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        double? GetDouble(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        double GetDoubleOrDefault(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        int? GetInt(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        int GetIntOrDefault(string columnName);
        
        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        string GetString(string columnName);

        /// <summary>
        /// Gets a value from the reader if one exists, or an empty string otherwise.
        /// </summary>
        string GetStringOrEmpty(string columnName);
        
        /// <summary>
        /// Moves to the next record.
        /// </summary>
        /// <returns>
        /// True if there is another record, false otherwise.
        /// </returns>
        bool MoveNext();
    }
}
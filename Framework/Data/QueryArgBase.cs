// <copyright filename="QueryArgBase.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Base query argument.
    /// </summary>
    /// <seealso cref="B1PP.Data.IQueryArg" />
    public abstract class QueryArgBase : IQueryArg
    {
        /// <summary>
        /// Gets the place holder.
        /// </summary>
        /// <remarks>
        /// Typically a @placeHolderName format is used, but you can use any placeholder.
        /// </remarks>
        public string PlaceHolder { get; protected set; }

        /// <summary>
        /// Gets the value to replace the placeholder with.
        /// </summary>
        public abstract string Value { get; }

        /// <summary>
        /// Simple escape function for a string.
        /// </summary>
        /// <param name="value">
        /// The string to escape.
        /// </param>
        /// <returns>
        /// The <see cref="string" /> escaped.
        /// </returns>
        protected string Escape(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value.Replace("'", "''");
        }

        /// <summary>
        /// Creates a new enumerable with single quote around the strings.
        /// </summary>
        /// <param name="list">
        /// An enumerable with the strings.
        /// </param>
        /// <returns>
        /// A new enumerable with all strings single quoted
        /// </returns>
        protected IEnumerable<string> SingleQuote(IEnumerable<string> list)
        {
            var quoted = new List<string>();
            list.ToList().ForEach(s => quoted.Add(SingleQuote(s)));

            return quoted;
        }

        /// <summary>
        /// Surrounds a string with single quotes.
        /// </summary>
        /// <param name="s">
        /// The string.
        /// </param>
        /// <returns>
        /// A new single quoted string.
        /// </returns>
        protected string SingleQuote(string s)
        {
            return $"'{s}'";
        }

        /// <summary>
        /// Converts a string into an SQL parameter string.
        /// </summary>
        /// <param name="value">
        /// The string with the value to be used.
        /// </param>
        /// <returns>
        /// An escaped and single quoted string representing the value.
        /// </returns>
        protected string ToSqlParameter(string value)
        {
            string escapedValue = Escape(value);
            string singleQuotedValue = SingleQuote(escapedValue);
            return ToUnicode(singleQuotedValue);
        }

        /// <summary>
        /// Converts a string into an SQL parameter string.
        /// </summary>
        /// <param name="value">
        /// The string with the value to be used.
        /// </param>
        /// <returns>
        /// An escaped and single quoted string representing the value.
        /// </returns>
        protected string ToSqlParameter(DateTime value)
        {
            string stringValue = value.ToString(@"yyyyMMdd");
            return SingleQuote(stringValue);
        }

        /// <summary>
        /// Converts a int into an SQL parameter string.
        /// </summary>
        /// <param name="value">
        /// The int with the value to be used.
        /// </param>
        /// <returns>
        /// An escaped and single quoted string representing the value.
        /// </returns>
        protected string ToSqlParameter(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a double into an SQL parameter string.
        /// </summary>
        /// <param name="value">
        /// The double with the value to be used.
        /// </param>
        /// <returns>
        /// An escaped and single quoted string representing the value.
        /// </returns>
        protected string ToSqlParameter(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds the unicode marker (N) to the string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value prefixed by N.
        /// </returns>
        protected string ToUnicode(string value)
        {
            return $@"N{value}";
        }

        /// <summary>
        /// Adds the unicode marker (N) to multiple string values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// All values prefixed by N.
        /// </returns>
        protected IEnumerable<string> ToUnicode(IEnumerable<string> values)
        {
            return values.Select(ToUnicode);
        }
    }
}
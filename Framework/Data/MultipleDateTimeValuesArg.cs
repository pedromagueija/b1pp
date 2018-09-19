// <copyright filename="MultipleDateTimeValuesArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents multiple date time values that can be used as parameters for a query.
    /// </summary>
    /// <remarks>
    /// The DateTime values will be converted automatically to the universal SQL date format ('yyyyMMdd').
    /// </remarks>
    /// <seealso cref="B1PP.Data.QueryArgBase" />
    internal class MultipleDateTimeValuesArg : QueryArgBase
    {
        private readonly IEnumerable<DateTime> values;

        /// <summary>
        /// Gets the value to replace the placeholder with.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}" /> as a <see cref="String" />, e.g.: '20010101', '20121231'
        /// </returns>
        public override string Value
        {
            get
            {
                var stringValues = new List<string>(values.Count());
                stringValues.AddRange(values.Select(ToSqlParameter));
                return string.Join(",", stringValues.ToArray());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleDateTimeValuesArg" /> class.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="values">The values.</param>
        public MultipleDateTimeValuesArg(string placeHolder, IEnumerable<DateTime> values) : base(placeHolder)
        {
            this.values = values;
        }
    }
}
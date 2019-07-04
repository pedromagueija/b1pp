// <copyright filename="DateTimeValueArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;

    /// <summary>
    /// Represents a query parameter of DateTime type.
    /// </summary>
    /// <remarks>
    /// The DateTime will be converted automatically to the universal SQL date format.
    /// </remarks>
    /// <seealso cref="QueryArgBase" />
    internal class DateTimeValueArg : QueryArgBase
    {
        private readonly DateTime value;

        /// <summary>
        /// Gets the value to replace the placeholder with.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime" /> as a <see cref="String" />.
        /// </returns>
        public override string Value
        {
            get
            {
                return ToSqlParameter(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeValueArg" /> class.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="value">The value.</param>
        public DateTimeValueArg(string placeHolder, DateTime value)  : base(placeHolder)
        {
            this.value = value;
        }
    }
}
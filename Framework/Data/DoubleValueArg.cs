// <copyright filename="DoubleValueArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    /// <summary>
    /// Represents a query parameter of type double.
    /// </summary>
    /// <remarks>
    /// The value will be represented as an invariant value (e.g.: 123.123).
    /// </remarks>
    internal class DoubleValueArg : QueryArgBase
    {
        private readonly double value;

        /// <summary>
        /// Gets the value to replace the placeholder with.
        /// </summary>
        public override string Value
        {
            get
            {
                return ToSqlParameter(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleValueArg" /> class.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="value">The value.</param>
        public DoubleValueArg(string placeHolder, double value) : base(placeHolder)
        {
            this.value = value;
        }
    }
}
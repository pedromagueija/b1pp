// <copyright filename="IntegerValueArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    /// <summary>
    /// Represents a query parameter of type int.
    /// </summary>
    /// <remarks>
    /// The value will be represented as an invariant value (e.g.: 123).
    /// </remarks>
    public class IntegerValueArg : QueryArgBase
    {
        private readonly int value;

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
        /// Initializes a new instance of the <see cref="IntegerValueArg"/> class.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="value">The value.</param>
        public IntegerValueArg(string placeHolder, int value)
        {
            PlaceHolder = placeHolder;
            this.value = value;
        }
    }
}
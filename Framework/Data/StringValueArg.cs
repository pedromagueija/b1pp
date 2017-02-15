// <copyright filename="StringValueArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    /// <summary>
    /// Represents a query parameter of type string.
    /// </summary>
    /// <remarks>
    /// The value will be quoted and escaped.
    /// </remarks>
    public class StringValueArg : QueryArgBase
    {
        private readonly string value;

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
        /// Initializes a new instance of the <see cref="StringValueArg"/> class.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="value">The value.</param>
        public StringValueArg(string placeHolder, string value)
        {
            PlaceHolder = placeHolder;
            this.value = value ?? string.Empty;
        }
    }
}
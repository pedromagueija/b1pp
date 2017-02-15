// <copyright filename="IQueryArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    /// <summary>
    /// Represents a query parameter composed of a placeholder,
    /// <para />
    /// which will be replaced by the value on the query statement.
    /// </summary>
    public interface IQueryArg
    {
        /// <summary>
        /// Gets the place holder.
        /// </summary>
        /// <remarks>
        /// Typically an @placeHolderName format is used, but you can use any placeholder.
        /// </remarks>
        string PlaceHolder { get; }

        /// <summary>
        /// Gets the value to replace the placeholder with.
        /// </summary>
        string Value { get; }
    }
}
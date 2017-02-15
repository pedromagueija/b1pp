// <copyright filename="Utilities.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System.Text.RegularExpressions;

    internal static class Utilities
    {
        /// <summary>
        /// Splits the text by caps e.g.: "SplitByCaps" becomes "Split By Caps".
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The text split by caps.
        /// </returns>
        public static string SplitByCaps(string text)
        {
            return Regex.Replace(text, "(\\B[A-Z0-9])", " $1");
        }
    }
}
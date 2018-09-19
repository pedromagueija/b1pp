// <copyright filename="DoubleExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.Common
{
    using System;
    using System.Globalization;

    public static class DoubleExtensions
    {
        /// <summary>
        /// The tolerance suitable for Business One, where the maximum decimal places is 6.
        /// </summary>
        private const double DefaultTolerance = 0.0000001;

        /// <summary>
        /// Returns the value as an invariant string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Value as an invariant string.
        /// </returns>
        public static string AsString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// True when the values are the same, False otherwise.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The value to compare with.</param>
        /// <param name="tolerance">Threshold where values are considered to be the same.</param>
        /// <returns>
        /// True when the values are the same, False otherwise.
        /// </returns>
        public static bool IsEqual(this double value, double other, double tolerance)
        {
            return Math.Abs(value - other) < tolerance;
        }

        /// <summary>
        /// True when the values are the same, False otherwise.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The value to compare with.</param>
        /// <returns>
        /// True when the values are the same, False otherwise.
        /// </returns>
        public static bool IsEqual(this double value, double other)
        {
            return IsEqual(value, other, DefaultTolerance);
        }
    }
}
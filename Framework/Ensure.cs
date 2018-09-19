// <copyright filename="Ensure.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP
{
    using System;

    internal static class Ensure
    {
        public static void NotNull(string parameterName, object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string parameterName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($@"{parameterName} cannot be null or empty.");
            }
        }
    }
}
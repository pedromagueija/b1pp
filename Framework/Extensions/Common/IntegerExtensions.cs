// <copyright filename="IntegerExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.Common
{
    public static class IntegerExtensions
    {
        public static bool InRange(this int value, int low, int high)
        {
            return value >= low && value <= high;
        }

        public static bool OutsideRange(this int value, int low, int high)
        {
            return !value.InRange(low, high);
        }
    }
}
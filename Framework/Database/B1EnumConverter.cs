// <copyright filename="B1EnumConverter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;

    internal class B1EnumConverter
    {
        /// <summary>
        /// Converts a string representation of an Enumeration value to its Enumeration value.
        /// </summary>
        /// <param name="type">Type of the Enumeration.</param>
        /// <param name="value">String representation of the Enumeration value.</param>
        /// <returns>An object that represents the Enumeration value.</returns>
        /// <exception cref="B1EnumConvertException">Thrown when the conversion fails.</exception>
        public object Convert(Type type, string value)
        {
            try
            {
                var option = Enum.Parse(type, value);
                return option;
            }
            catch (Exception ex)
                when (ex is ArgumentNullException || ex is ArgumentException || ex is OverflowException)
            {
                var e = new B1EnumConvertException($"Cannot convert '{value}' into enumeration of type '{type}'.", ex);
                e.Data.Add("value", value);
                e.Data.Add("type", type);

                throw e;
            }
        }
    }
}
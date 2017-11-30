// <copyright filename="EnumConverter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Collections.Generic;

    internal class EnumConverter
    {
        private readonly Type type;

        public EnumConverter(Type type)
        {
            this.type = type;
        }

        public IEnumerable<Tuple<string, string>> ToEnumerable()
        {
            var values = Enum.GetValues(type);
            var underlyingType = Enum.GetUnderlyingType(type);

            foreach (var value in values)
            {
                var underlyingValue = Convert.ChangeType(value, underlyingType);
                var key = Convert.ToString(underlyingValue);
                var name = Enum.GetName(type, value);
                yield return new Tuple<string, string>(key, name);
            }
        }
    }
}
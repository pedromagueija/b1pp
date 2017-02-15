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
            Array values = Enum.GetValues(type);
            Type underlyingType = Enum.GetUnderlyingType(type);

            foreach (object value in values)
            {
                object underlyingValue = Convert.ChangeType(value, underlyingType);
                string key = Convert.ToString(underlyingValue);
                string name = Enum.GetName(type, value);
                yield return new Tuple<string, string>(key, name);
            }
        }
    }
}
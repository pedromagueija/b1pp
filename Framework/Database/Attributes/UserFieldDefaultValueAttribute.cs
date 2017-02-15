// <copyright filename="UserFieldDefaultValueAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database.Attributes
{
    using System;
    using System.Globalization;

    using SAPbobsCOM;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UserFieldDefaultValueAttribute : Attribute
    {
        private readonly object value;

        public UserFieldDefaultValueAttribute(int value)
        {
            this.value = value;
        }

        public UserFieldDefaultValueAttribute(double value)
        {
            this.value = value;
        }

        public UserFieldDefaultValueAttribute(string value)
        {
            this.value = value ?? string.Empty;
        }

        public void Apply(UserFieldsMD field)
        {
            string text = string.Empty;

            if (value is int)
                text = ((int) value).ToString(CultureInfo.InvariantCulture);
            if (value is double)
                text = ((double) value).ToString(CultureInfo.InvariantCulture);
            if (value is string)
                text = (string)value;

            if (!string.IsNullOrEmpty(text))
                field.DefaultValue = text;
        }
    }
}
// <copyright filename="UserFieldDefaultValueAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;
    using System.Globalization;

    using SAPbobsCOM;

    /// <summary>
    /// Defines the default value to use for a UserField.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class UserFieldDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// The value
        /// </summary>
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldDefaultValueAttribute" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UserFieldDefaultValueAttribute(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldDefaultValueAttribute" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UserFieldDefaultValueAttribute(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldDefaultValueAttribute" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UserFieldDefaultValueAttribute(string value)
        {
            this.value = value ?? string.Empty;
        }

        /// <summary>
        /// Applies the default value to the specified field.
        /// </summary>
        /// <param name="field">The field to apply the default value to.</param>
        internal void Apply(UserFieldsMD field)
        {
            var text = string.Empty;

            if (value is int)
            {
                text = ((int) value).ToString(CultureInfo.InvariantCulture);
            }

            if (value is double)
            {
                text = ((double) value).ToString(CultureInfo.InvariantCulture);
            }

            if (value is string)
            {
                text = (string) value;
            }

            if (!string.IsNullOrEmpty(text))
            {
                field.DefaultValue = text;
            }
        }
    }
}
// <copyright filename="UserFieldValidValueAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Defines the valid values for a user field.
    /// </summary>
    /// <remarks>
    /// This attribute can be used multiple times for different values.
    /// </remarks>
    /// <seealso cref="UserFieldEnumValuesAttribute"/>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class UserFieldValidValueAttribute : Attribute
    {
        public string Value { get; }
        public string Description { get; }

        public UserFieldValidValueAttribute(string value, string description)
        {
            Value = value;
            Description = description;
        }

        internal void Apply(UserFieldsMD field)
        {
            var validValues = field.ValidValues;

            validValues.Value = Value;
            validValues.Description = Description;
            validValues.Add();
        }
    }
}
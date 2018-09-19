// <copyright filename="UserFieldValidValueAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class UserFieldValidValueAttribute : Attribute
    {
        public string Value { get; }
        public string Description { get; }

        public UserFieldValidValueAttribute(string value, string description)
        {
            Value = value;
            Description = description;
        }

        public void Apply(UserFieldsMD field)
        {
            var validValues = field.ValidValues;

            validValues.Value = Value;
            validValues.Description = Description;
            validValues.Add();
        }
    }
}
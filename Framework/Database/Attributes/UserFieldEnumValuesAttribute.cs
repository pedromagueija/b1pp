// <copyright filename="UserFieldEnumValuesAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Allows using an enumeration type as the list of valid values for a user field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UserFieldEnumValuesAttribute : Attribute
    {
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldEnumValuesAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of the enumeration.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the type passed in is not an Enum.
        /// </exception>
        public UserFieldEnumValuesAttribute(Type type)
        {            
            // check type is an enum
            if(!type.IsEnum)
                throw new ArgumentException($@"'{type}' is not an Enum type.");

            this.type = type;
        }

        public void Apply(UserFieldsMD field)
        {
            var values = new EnumConverter(type).ToEnumerable();
            ValidValuesMD validValues = field.ValidValues;

            foreach (var value in values)
            {                
                validValues.Value = value.Item1;
                validValues.Description = value.Item2;
                validValues.Add();
            }
        }
    }
}
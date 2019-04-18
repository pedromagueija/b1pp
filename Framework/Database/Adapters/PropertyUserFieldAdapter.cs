// <copyright filename="PropertyUserFieldAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>


namespace B1PP.Database.Adapters
{
    using System;
    using System.Reflection;
    using Attributes;
    using SAPbobsCOM;

    /// <summary>
    /// Converts a property into a user field.
    /// </summary>
    internal class PropertyUserFieldAdapter
    {
        private readonly UserFieldsMD field;
        private readonly PropertyInfo property;
        private readonly string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyUserFieldAdapter" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="property">The property.</param>
        /// <param name="field">The field.</param>
        public PropertyUserFieldAdapter(string tableName, PropertyInfo property, UserFieldsMD field)
        {
            this.tableName = tableName;
            this.property = property;
            this.field = field;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the <see cref="UserFieldAttribute" /> is not present in the given property.
        /// </exception>
        public void Execute()
        {
            var userFieldTypeAttribute = property.GetCustomAttribute<UserFieldAttribute>();
            if (userFieldTypeAttribute == null)
            {
                throw new InvalidOperationException(
                    $"UserFieldAttribute is missing from property {property.DeclaringType}.{property.Name}.");
            }

            field.TableName = tableName;

            var userFieldNameAttr = GetUserFieldNameAttribute();
            userFieldNameAttr.Apply(field, property);

            userFieldTypeAttribute.Apply(field);

            // by default all fields are mandatory
            field.Mandatory = BoYesNoEnum.tYES;

            // overrides mandatory status if optional attribute is present
            var userFieldOptional = property.GetCustomAttribute<UserFieldOptionalAttribute>();
            userFieldOptional?.Apply(field);

            var defaultValueAttr = property.GetCustomAttribute<UserFieldDefaultValueAttribute>();
            defaultValueAttr?.Apply(field);

            var enumValuesAttr = property.GetCustomAttribute<UserFieldEnumValuesAttribute>();
            enumValuesAttr?.Apply(field);

            var userFieldValidValues = property.GetCustomAttributes<UserFieldValidValueAttribute>();
            foreach (var validValueAttribute in userFieldValidValues)
            {
                validValueAttribute.Apply(field);
            }
        }

        private FieldNameAttribute GetUserFieldNameAttribute()
        {
            return property.GetCustomAttribute<FieldNameAttribute>() ??
                   new FieldNameAttribute(property.Name, Utilities.SplitByCaps(property.Name));
        }
    }
}
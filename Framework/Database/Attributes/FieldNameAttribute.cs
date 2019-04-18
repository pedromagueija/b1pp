// <copyright filename="FieldNameAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;
    using System.Reflection;

    using SAPbobsCOM;

    /// <summary>
    /// Sets the name and description of a user field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; }

        /// <summary>
        /// Gets the field description.
        /// </summary>
        /// <value>
        /// The field description.
        /// </value>
        public string FieldDescription { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNameAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">The field description.</param>
        public FieldNameAttribute(string fieldName, string fieldDescription)
        {
            FieldName = fieldName;
            FieldDescription = fieldDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNameAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public FieldNameAttribute(string fieldName)
        {
            FieldName = fieldName;
            FieldDescription = string.Empty;
        }

        internal void Apply(UserFieldsMD field, PropertyInfo property)
        {
            // TODO: take into consideration that FieldName may contain U_FieldName and that will yield a U_U_FieldName field
            field.Name = DetermineFieldName(property);
            field.Description = DetermineFieldDescription(property);
        }

        private string DetermineFieldDescription(PropertyInfo property)
        {
            if (string.IsNullOrWhiteSpace(FieldDescription))
            {
                return Utilities.SplitByCaps(property.Name);
            }

            return FieldDescription;
        }

        private string DetermineFieldName(PropertyInfo property)
        {
            if (string.IsNullOrWhiteSpace(FieldName))
            {
                return property.Name;
            }

            return FieldName;
        }
    }
}
// <copyright filename="UserFieldOptionalAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Allows marking of a field as being optional (nullable on the database).
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class UserFieldOptionalAttribute : Attribute
    {
        /// <summary>
        /// Applies the optional attribute to the specified field.
        /// </summary>
        /// <param name="field">The field to make optional.</param>
        public void Apply(UserFieldsMD field)
        {
            field.Mandatory = BoYesNoEnum.tNO;
        }
    }
}
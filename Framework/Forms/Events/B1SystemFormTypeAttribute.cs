// <copyright filename="B1SystemFormTypeAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;

    /// <summary>
    /// Use this attribute to mark a class with the system form type it represents.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class B1SystemFormTypeAttribute : Attribute
    {
        public string FormType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1SystemFormTypeAttribute" /> class.
        /// </summary>
        /// <param name="formType">Type of the form.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="formType" /> is null.
        /// </exception>
        public B1SystemFormTypeAttribute(string formType)
        {
            if (string.IsNullOrEmpty(formType))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(formType));
            }

            FormType = formType;
        }
    }
}
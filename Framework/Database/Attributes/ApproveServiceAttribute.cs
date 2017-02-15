// <copyright filename="ApproveServiceAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Activates the user defined object service "Approve".
    /// </summary>
    /// <seealso cref="System.Attribute" />
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ApproveServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets the workflow manager template identifier.
        /// </summary>
        /// <value>
        /// The workflow manager template identifier.
        /// </value>
        public string TemplateId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApproveServiceAttribute"/> class.
        /// </summary>
        /// <param name="templateId">The workflow manager template identifier.</param>
        public ApproveServiceAttribute(string templateId)
        {
            TemplateId = templateId ?? string.Empty;
        }

        /// <summary>
        /// Activates the approve service.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        internal void Apply(UserObjectsMD userObject)
        {
            userObject.CanApprove = BoYesNoEnum.tYES;
            userObject.TemplateID = TemplateId;
        }
    }
}
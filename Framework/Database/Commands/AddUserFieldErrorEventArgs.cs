// <copyright filename="UserFieldErrorEventArgs.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;

namespace B1PP.Database.Commands
{
    /// <summary>
    /// Contains details about errors regarding actions with UserFields.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class AddUserFieldErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// The error description.
        /// </value>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public int ErrorCode { get; set; }
    }
}
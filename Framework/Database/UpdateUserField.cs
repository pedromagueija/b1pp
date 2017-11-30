// <copyright filename="UpdateUserField.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbobsCOM;

    /// <summary>
    /// Updates a user field on the database.
    /// </summary>
    /// <remarks>
    /// The user field object is released after being added
    /// according to SAP recommendations.
    /// </remarks>
    internal class UpdateUserField
    {
        private readonly Company company;
        private readonly UserFieldsMD field;

        public UpdateUserField(Company company, UserFieldsMD field)
        {
            this.company = company;
            this.field = field;
        }

        /// <summary>
        /// Occurs when the update fails.
        /// </summary>
        public event EventHandler<UserFieldErrorEventArgs> OnError = delegate { };

        /// <summary>
        /// Executes the update action and releases the object.
        /// </summary>
        public void Execute()
        {
            var result = field.Add();
            if (result != 0)
            {
                var errorArgs = new UserFieldErrorEventArgs
                {
                    TableName = field.TableName,
                    FieldName = field.Name,
                    ErrorCode = company.GetLastErrorCode(),
                    ErrorDescription = company.GetLastErrorDescription()
                };

                OnError(this, errorArgs);
            }

            Marshal.ReleaseComObject(field);
        }
    }
}
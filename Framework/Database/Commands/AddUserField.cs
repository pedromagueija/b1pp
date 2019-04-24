// <copyright filename="AddUserField.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace B1PP.Database.Commands
{
    /// <summary>
    /// Adds a user field to the database.
    /// </summary>
    /// <remarks>
    /// The user field object is released after being added
    /// according to SAP recommendations.
    /// </remarks>
    internal class AddUserField
    {
        private readonly Company company;
        private readonly UserFieldsMD field;

        public AddUserField(Company company, UserFieldsMD field)
        {
            this.company = company;
            this.field = field;
        }

        public void Execute()
        {
            var result = field.Add();
            if (result != 0)
            {
                var errorArgs = new AddUserFieldErrorEventArgs
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

        public event EventHandler<AddUserFieldErrorEventArgs> OnError = delegate { };
    }
}
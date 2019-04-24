// <copyright filename="AddUserTable.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace B1PP.Database.Commands
{
    internal class AddUserTable
    {
        public IEnumerable<string> Errors => errors;

        private readonly Company company;
        private readonly UserTablesMD table;
        private readonly List<string> errors = new List<string>();

        public AddUserTable(Company company, UserTablesMD table)
        {
            this.company = company;
            this.table = table;
        }

        public event EventHandler<AddUserTableErrorEventArgs> Error = delegate { };

        public bool Execute()
        {
            int result = table.Add();
            if (result != 0)
            {
                errors.Add(company.GetLastErrorDescription());
                
                var errorArgs = new AddUserTableErrorEventArgs
                {
                    TableName = table.TableName,
                    ErrorCode = company.GetLastErrorCode(),
                    ErrorDescription = company.GetLastErrorDescription()
                };

                Error(this, errorArgs);
                
                return false;
            }

            Marshal.ReleaseComObject(table);

            return true;
        }
    }
}
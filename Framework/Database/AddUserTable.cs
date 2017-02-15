// <copyright filename="AddUserTable.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbobsCOM;

    internal class AddUserTable
    {
        private readonly Company company;
        private readonly UserTablesMD table;

        public AddUserTable(Company company, UserTablesMD table)
        {
            this.company = company;
            this.table = table;
        }

        public event EventHandler<AddUserTableErrorEventArgs> Error = delegate { };

        public void Execute()
        {
            int result = table.Add();
            if (result != 0)
            {
                var errorArgs = new AddUserTableErrorEventArgs
                {
                    TableName = table.TableName,
                    ErrorCode = company.GetLastErrorCode(),
                    ErrorDescription = company.GetLastErrorDescription()
                };

                Error(this, errorArgs);
            }

            Marshal.ReleaseComObject(table);
        }
    }
}
// <copyright filename="AddUserObject.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbobsCOM;

    internal class AddUserObject
    {
        private readonly Company company;
        private readonly UserObjectsMD userObjectMd;

        public AddUserObject(Company company, UserObjectsMD userObjectMd)
        {
            this.company = company;
            this.userObjectMd = userObjectMd;
        }

        public event EventHandler<AddUserObjectErrorEventArgs> OnError = delegate { };

        public void Execute()
        {
            var result = userObjectMd.Add();
            if (result != 0)
            {
                var errorArgs = new AddUserObjectErrorEventArgs
                {
                    TableName = userObjectMd.TableName,
                    ObjectName = userObjectMd.Name,
                    ErrorCode = company.GetLastErrorCode(),
                    ErrorDescription = company.GetLastErrorDescription()
                };

                OnError(this, errorArgs);
            }

            Marshal.ReleaseComObject(userObjectMd);
        }
    }
}
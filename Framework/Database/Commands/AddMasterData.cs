// <copyright filename="AddMasterData.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Reflection;
using B1PP.Database.Attributes;
using SAPbobsCOM;

namespace B1PP.Database.Commands
{
    using System.Runtime.InteropServices;

    internal class AddMasterData<T> : IAddMasterData<T> where T : class, IMasterDataRecord
    {
        private readonly Company company;
        private readonly IUdoSerializer<T> serializer;

        public AddMasterData(Company company, IUdoSerializer<T> serializer)
        {
            this.company = company ?? throw new ArgumentNullException(nameof(company));
            this.serializer = serializer;
        }

        /// <summary>
        /// True when the action completes with success, false otherwise.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Contains the error, if any, that caused the action to fail.
        /// </summary>
        public string Error { get; private set; }

        public string Execute(T instance)
        {
            string udoId = GetUserObjectId();
            string xml = serializer.Serialize(instance);

            var service = company.GetCompanyService().GetGeneralService(udoId);
            var data = (GeneralData) service.GetDataInterfaceFromXMLString(xml);

            string code;
            try
            {
                var result = service.Add(data);
                code = result.GetProperty(@"Code") as string;
                Success = true;
            }
            catch (COMException e)
            {
                if (e.HResult == -2035)
                {
                    Error = @"This entry already exists in the database.";
                    Success = false;
                    return string.Empty;
                }
                
                throw;
            }

            return code;
        }

        private string GetUserObjectId()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<UserObjectAttribute>();
            if (attr == null)
            {
                throw new ArgumentException($@"'{type}' does not have a user object attribute.");
            }

            return attr.ObjectId;
        }
    }
}
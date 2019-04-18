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
    internal class AddMasterData<T> : IAddMasterData<T> where T : class, IMasterDataRecord
    {
        private readonly Company company;
        private readonly IUdoSerializer<T> serializer;

        public AddMasterData(Company company, IUdoSerializer<T> serializer)
        {
            this.company = company ?? throw new ArgumentNullException(nameof(company));
            this.serializer = serializer;
        }

        public string Invoke(T instance)
        {
            string udoId = GetUserObjectId();
            string xml = serializer.Serialize(instance);

            var service = company.GetCompanyService().GetGeneralService(udoId);
            var data = (GeneralData) service.GetDataInterfaceFromXMLString(xml);
            var result = service.Add(data);

            string code = result.GetProperty(@"Code") as string;
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
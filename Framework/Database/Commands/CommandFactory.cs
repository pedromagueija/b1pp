// <copyright filename="CommandFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using SAPbobsCOM;

namespace B1PP.Database.Commands
{
    public static class CommandFactory
    {
        public static IAddMasterData<T> AddMasterData<T>(Company company) where T : class, IMasterDataRecord
        {
            var serializer = (IUdoSerializer<T>) new GenericUdoSerializer<T>();
            return new AddMasterData<T>(company, serializer);
        }
    }
}
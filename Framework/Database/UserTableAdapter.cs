// <copyright filename="UserTableAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System.Xml.Linq;

    using SAPbobsCOM;

    internal class UserTableAdapter : AdapterBase
    {
        private readonly IUserTablesMD table;
        private readonly XElement userTable;

        public UserTableAdapter(IUserTablesMD table, XElement userTable)
        {
            this.table = table;
            this.userTable = userTable;
        }

        public void Execute()
        {
            var attributes = userTable.Attributes();

            PopulateProperties<IUserTablesMD>(attributes, table);
        }
    }
}
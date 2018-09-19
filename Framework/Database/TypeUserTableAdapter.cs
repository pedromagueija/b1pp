// <copyright filename="TypeUserTableAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System;
    using System.Reflection;

    using Attributes;

    using SAPbobsCOM;

    internal class TypeUserTableAdapter
    {
        private readonly UserTablesMD table;
        private readonly Type type;

        public TypeUserTableAdapter(Type type, UserTablesMD table)
        {
            this.type = type;
            this.table = table;
        }

        public void Execute()
        {
            var userTableData = type.GetCustomAttribute<UserTableAttribute>();
            if (userTableData != null)
            {
                userTableData.Apply(type, table);

                var archiveable = type.GetCustomAttribute<ArchiveDateFieldAttribute>();
                archiveable?.Apply(table);
            }
        }
    }
}
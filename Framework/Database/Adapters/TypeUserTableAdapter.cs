// <copyright filename="TypeUserTableAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Reflection;
using B1PP.Database.Attributes;
using SAPbobsCOM;

namespace B1PP.Database.Adapters
{
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
                userTableData.Apply(table);

                var archivable = type.GetCustomAttribute<ArchiveDateFieldAttribute>();
                archivable?.Apply(table);
                return;
            }

            var childTableAttr = type.GetCustomAttribute<ChildUserTableAttribute>();
            if (childTableAttr != null)
            {
                childTableAttr.Apply(table);
            }
        }
    }
}
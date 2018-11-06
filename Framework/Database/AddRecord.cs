// <copyright filename="AddRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Attributes;

    using SAPbobsCOM;

    public class AddRecord<T> where T : INoObjectRecord
    {
        private readonly Company company;

        public AddRecord(Company company)
        {
            this.company = company;
        }
        
        private Func<PropertyInfo, bool> HasUserFieldAttribute
        {
            get
            {
                return p => p.GetCustomAttribute<UserFieldAttribute>() != null;
            }
        }

        public int Invoke(T instance)
        {
            var table = company.UserTables.Item(GetTableName());
            var userFields = table.UserFields.Fields;
            var properties = GetType().GetProperties().Where(HasUserFieldAttribute);
            foreach (var property in properties)
            {
                string name = GetName(property);
                var value = property.GetValue(instance) ?? string.Empty;

                userFields.Item(name).Value = value;
            }

            return table.Add();
        }

        private string GetName(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<FieldNameAttribute>();
            if (attr == null)
            {
                return $@"U_{property.Name}";
            }

            return $@"U_{attr.FieldName}";
        }

        private string GetTableName()
        {
            var attr = typeof(T).GetCustomAttribute<UserTableAttribute>();
            return attr.TableName;
        }
    }
}
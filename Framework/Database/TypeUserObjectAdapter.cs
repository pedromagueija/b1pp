// <copyright filename="TypeUserObjectAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Attributes;

    using SAPbobsCOM;

    internal class TypeUserObjectAdapter
    {
        private readonly List<Type> ignoredTypes = new List<Type>
        {
            typeof(SimpleRecord),
            typeof(DocumentRecord),
            typeof(DocumentRecordLine)
        };

        private readonly string tableName;
        private readonly Type type;
        private readonly UserObjectsMD userObject;

        public TypeUserObjectAdapter(Type type, string tableName, UserObjectsMD userObject)
        {
            this.type = type;
            this.tableName = tableName;
            this.userObject = userObject;
        }

        public void Execute()
        {
            var userObjectAttr = type.GetCustomAttribute<UserObjectAttribute>();
            userObjectAttr.Apply(userObject, tableName);

            // enable other services
            var userObjectServicesAttr = type.GetCustomAttribute<UserObjectServicesAttribute>();
            userObjectServicesAttr.Apply(userObject);

            var approveServiceAttr = type.GetCustomAttribute<ApproveServiceAttribute>();
            approveServiceAttr?.Apply(userObject);

            var properties = GetProperties(type);

            foreach (var property in properties)
            {
                SetFindColumns(property);

                SetChildTables(property);
            }

            userObject.CanCreateDefaultForm = BoYesNoEnum.tNO;
            userObject.MenuUID = string.Empty;
            userObject.EnableEnhancedForm = BoYesNoEnum.tNO;
        }

        private IEnumerable<PropertyInfo> GetProperties(Type userTable)
        {
            return userTable.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private void SetChildTables(PropertyInfo property)
        {
            var childTableAttribute = property.GetCustomAttribute<ChildUserTableAttribute>();
            if (childTableAttribute == null)
            {
                return;
            }

            var child = userObject.ChildTables;
            child.TableName = childTableAttribute.TableName;
            child.ObjectName = childTableAttribute.ObjectName;

            // setting the child table log when the object doesn't support the log service 
            // results in an error in transaction when updating the object
            if (userObject.CanLog == BoYesNoEnum.tYES)
            {
                child.LogTableName = childTableAttribute.LogTableName;
            }

            child.Add();
        }

        private void SetFindColumn(PropertyInfo property)
        {
            var userFieldNameAttr = property.GetCustomAttribute<FieldNameAttribute>();
            var findColumns = userObject.FindColumns;

            if (userFieldNameAttr != null)
            {
                findColumns.ColumnAlias = ignoredTypes.Contains(property.DeclaringType)
                    ? userFieldNameAttr.FieldName
                    : $"U_{userFieldNameAttr.FieldName}";
                findColumns.ColumnDescription =
                    string.IsNullOrEmpty(userFieldNameAttr.FieldDescription)
                        ? SplitByCaps(property.Name)
                        : userFieldNameAttr.FieldDescription;
            }
            else
            {
                findColumns.ColumnAlias = ignoredTypes.Contains(property.DeclaringType)
                    ? property.Name
                    : $"U_{property.Name}";
                findColumns.ColumnDescription = SplitByCaps(property.Name);
            }
        }

        private void SetFindColumns(PropertyInfo property)
        {
            if (!property.CanWrite)
            {
                return;
            }

            var excludeColumnAttr = property.GetCustomAttribute<ExcludeFindColumnAttribute>();
            if (excludeColumnAttr == null)
            {
                SetFindColumn(property);

                userObject.FindColumns.Add();
            }
        }

        private string SplitByCaps(string value)
        {
            return Regex.Replace(value, "(\\B[A-Z0-9])", " $1");
        }
    }
}
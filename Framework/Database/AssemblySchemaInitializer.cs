// <copyright filename="AssemblySchemaInitializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Attributes;

    using SAPbobsCOM;

    public class AssemblySchemaInitializer
    {
        private readonly Assembly assembly;
        private readonly Company company;

        private readonly List<Type> ignoredTypes = new List<Type>
        {
            typeof(SimpleRecord),
            typeof(DocumentRecord),
            typeof(DocumentRecordLine)
        };

        public AssemblySchemaInitializer(Company company, Assembly assembly)
        {
            this.company = company;
            this.assembly = assembly;
        }

        public event EventHandler<UserFieldErrorEventArgs> CreateUserFieldError = delegate { };
        public event EventHandler<AddUserObjectErrorArgs> CreateUserObjectError = delegate { };
        public event EventHandler<AddUserTableErrorEventArgs> CreateUserTableError = delegate { };

        public void Initialize()
        {
            var userTableTypes = GetUserTableTypes().ToList();
            var typeMap = new Dictionary<string, Type>();

            /*
             * Tables are created all first, then all fields to prevent 
             * the situation where a table or field are missing but 
             * are necessary to create the object.
             */

            foreach (Type type in userTableTypes)
            {
                string tableName = CreateTable(type);
                typeMap.Add(tableName, type);
            }

            foreach (string tableName in typeMap.Keys)
            {
                CreateFields(typeMap[tableName], tableName);
            }

            foreach (string tableName in typeMap.Keys)
            {
                CreateObject(typeMap[tableName], tableName);
            }
        }

        private void CreateFields(Type type, string tableName)
        {
            var properties = GetCustomProperties(type);
            var annotated = properties.Where(HasUserFieldAttribute);

            foreach (PropertyInfo property in annotated)
            {
                var field = (UserFieldsMD) company.GetBusinessObject(BoObjectTypes.oUserFields);
                var adapter = new PropertyUserFieldAdapter(tableName, property, field);
                adapter.Execute();

                var addUserField = new AddUserField(company, field);
                addUserField.OnError += OnAddUserFieldError;
                addUserField.Execute();
            }
        }

        private bool HasUserFieldAttribute(PropertyInfo p)
        {
            return p.GetCustomAttribute<UserFieldAttribute>() != null;
        }

        private void CreateObject(Type type, string tableName)
        {
            // ignore if not marked with user object attribute
            var userObjectAttribute = type.GetCustomAttribute<UserObjectAttribute>();
            if (userObjectAttribute == null)
            {
                return;
            }

            var userObject = (UserObjectsMD) company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var adapter = new TypeUserObjectAdapter(type, tableName, userObject);
            adapter.Execute();

            var addUserObject = new AddUserObject(company, userObject);
            addUserObject.OnError += OnAddUserObjectError;
            addUserObject.Execute();
        }

        private string CreateTable(Type type)
        {
            var table = (UserTablesMD) company.GetBusinessObject(BoObjectTypes.oUserTables);
            var adapter = new TypeUserTableAdapter(type, table);
            adapter.Execute();

            // used to return the table name, because the AddUserTable action will release the table
            // thus we won't have access to the table.TableName anymore
            string tableName = table.TableName;

            var addUserTable = new AddUserTable(company, table);
            addUserTable.Error += OnAddUserTableError;
            addUserTable.Execute();

            return tableName;
        }

        private IEnumerable<PropertyInfo> GetCustomProperties(Type userTable)
        {
            return GetWriteableProperties(userTable).Where(p => !ignoredTypes.Contains(p.DeclaringType));
        }

        private IEnumerable<PropertyInfo> GetProperties(Type userTable)
        {
            return userTable.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private IEnumerable<Type> GetUserTableTypes()
        {
            Type userTableAttribute = typeof(UserTableAttribute);

            return assembly.GetTypes().Where(
                t => t.IsClass &&
                     !t.IsAbstract &&
                     t.GetCustomAttributes(userTableAttribute).Any());
        }

        private IEnumerable<PropertyInfo> GetWriteableProperties(Type userTable)
        {
            return GetProperties(userTable)
                .Where(p => p.CanWrite);
        }

        private void OnAddUserFieldError(object sender, UserFieldErrorEventArgs e)
        {
            CreateUserFieldError(sender, e);
        }

        private void OnAddUserObjectError(object sender, AddUserObjectErrorArgs e)
        {
            CreateUserObjectError(sender, e);
        }

        private void OnAddUserTableError(object sender, AddUserTableErrorEventArgs e)
        {
            CreateUserTableError(sender, e);
        }
    }
}
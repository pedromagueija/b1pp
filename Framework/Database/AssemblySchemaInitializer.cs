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

    /// <summary>
    /// Creates the schema using the models attributes as a source.
    /// </summary>
    public class AssemblySchemaInitializer
    {
        /// <summary>
        /// The assembly that contains the models.
        /// </summary>
        private readonly Assembly assembly;

        /// <summary>
        /// The company object.
        /// </summary>
        private readonly Company company;

        /// <summary>
        /// Types in this list will have their properties ignored when creating fields.
        /// </summary>
        private readonly List<Type> ignoredTypes = new List<Type>
        {
            typeof(SimpleRecord),
            typeof(DocumentRecord),
            typeof(DocumentRecordLine)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblySchemaInitializer" /> class.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="assembly">The assembly.</param>
        public AssemblySchemaInitializer(Company company, Assembly assembly)
        {
            this.company = company;
            this.assembly = assembly;
        }

        /// <summary>
        /// Occurs when [create user field error].
        /// </summary>
        public event EventHandler<UserFieldErrorEventArgs> CreateUserFieldError = delegate { };

        /// <summary>
        /// Occurs when [create user object error].
        /// </summary>
        public event EventHandler<AddUserObjectErrorEventArgs> CreateUserObjectError = delegate { };

        /// <summary>
        /// Occurs when [create user table error].
        /// </summary>
        public event EventHandler<AddUserTableErrorEventArgs> CreateUserTableError = delegate { };

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            var userTableTypes = GetUserTableTypes().ToList();
            var typeMap = new Dictionary<string, Type>();

            /*
             * Tables are all created first, then all fields to prevent 
             * the situation where a table or field are missing but 
             * are necessary to create the object.
             */

            foreach (var type in userTableTypes)
            {
                var tableName = CreateTable(type);
                typeMap.Add(tableName, type);
            }

            foreach (var tableName in typeMap.Keys)
            {
                CreateFields(typeMap[tableName], tableName);
            }

            foreach (var tableName in typeMap.Keys)
            {
                CreateObject(typeMap[tableName], tableName);
            }
        }

        private void CreateField(string tableName, PropertyInfo property)
        {
            var field = (UserFieldsMD) company.GetBusinessObject(BoObjectTypes.oUserFields);
            var adapter = new PropertyUserFieldAdapter(tableName, property, field);
            adapter.Execute();

            var addUserField = new AddUserField(company, field);
            addUserField.OnError += OnAddUserFieldError;
            addUserField.Execute();
        }

        /// <summary>
        /// Creates the fields.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tableName">Name of the table.</param>
        private void CreateFields(Type type, string tableName)
        {
            var properties = GetCustomProperties(type);
            var annotated = properties.Where(HasUserFieldAttribute);

            foreach (var property in annotated)
            {
                CreateField(tableName, property);
            }
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tableName">Name of the table.</param>
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

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string CreateTable(Type type)
        {
            var table = (UserTablesMD) company.GetBusinessObject(BoObjectTypes.oUserTables);
            var adapter = new TypeUserTableAdapter(type, table);
            adapter.Execute();

            // used to return the table name, because the AddUserTable action will release the table
            // thus we won't have access to the table.TableName anymore
            var tableName = table.TableName;

            var addUserTable = new AddUserTable(company, table);
            addUserTable.Error += OnAddUserTableError;
            addUserTable.Execute();

            return tableName;
        }

        /// <summary>
        /// Gets the custom properties.
        /// </summary>
        /// <param name="userTable">The user table.</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetCustomProperties(Type userTable)
        {
            return GetWriteableProperties(userTable).Where(p => !ignoredTypes.Contains(p.DeclaringType));
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="userTable">The user table.</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetProperties(Type userTable)
        {
            return userTable.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets the user table types.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetUserTableTypes()
        {
            var userTableAttribute = typeof(UserTableAttribute);

            return assembly.GetTypes().Where(
                t => t.IsClass &&
                     !t.IsAbstract &&
                     t.GetCustomAttributes(userTableAttribute).Any());
        }

        /// <summary>
        /// Gets the writeable properties.
        /// </summary>
        /// <param name="userTable">The user table.</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetWriteableProperties(Type userTable)
        {
            return GetProperties(userTable)
                .Where(p => p.CanWrite);
        }

        /// <summary>
        /// Determines whether the property has a user field attribute.
        /// </summary>
        /// <param name="p">The property.</param>
        /// <returns>
        /// <c>true</c> if the property has a user field attribute; otherwise, <c>false</c>.
        /// </returns>
        private bool HasUserFieldAttribute(PropertyInfo p)
        {
            return p.GetCustomAttribute<UserFieldAttribute>() != null;
        }

        /// <summary>
        /// Called when [add user field error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UserFieldErrorEventArgs" /> instance containing the event data.</param>
        private void OnAddUserFieldError(object sender, UserFieldErrorEventArgs e)
        {
            CreateUserFieldError(sender, e);
        }

        /// <summary>
        /// Called when [add user object error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AddUserObjectErrorEventArgs" /> instance containing the event data.</param>
        private void OnAddUserObjectError(object sender, AddUserObjectErrorEventArgs e)
        {
            CreateUserObjectError(sender, e);
        }

        /// <summary>
        /// Called when [add user table error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AddUserTableErrorEventArgs" /> instance containing the event data.</param>
        private void OnAddUserTableError(object sender, AddUserTableErrorEventArgs e)
        {
            CreateUserTableError(sender, e);
        }
    }
}
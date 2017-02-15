// <copyright filename="SchemaManager.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;

    using SAPbobsCOM;

    public class SchemaManager
    {
        private readonly Company company;

        public SchemaManager(Company company)
        {
            this.company = company;
        }

        public event EventHandler<UserFieldErrorEventArgs> CreateUserFieldError = delegate { };
        public event EventHandler<AddUserObjectErrorArgs> CreateUserObjectError = delegate { };
        public event EventHandler<AddUserTableErrorEventArgs> CreateUserTableError = delegate { };

        public void InitializeFromAssembly(Assembly assembly)
        {
            var initializer = new AssemblySchemaInitializer(company, assembly);
            initializer.CreateUserTableError += OnAddUserTableError;
            initializer.CreateUserFieldError += OnAddUserFieldError;
            initializer.CreateUserObjectError += OnAddUserObjectError;
            initializer.Initialize();
        }

        public void InitializeFromFile(string fileName, Assembly assembly)
        {
            string contents = ReadFile(fileName, assembly);
            XDocument xml = XDocument.Parse(contents);

            CreateTablesFromXDocument(xml);
            CreateFieldsFromXDocument(xml);
            CreateObjectsFormXDocument(xml);
        }

        private void CreateFieldsFromXDocument(XDocument xml)
        {
            var userFields = xml.Descendants(@"UserField");

            foreach (XElement userField in userFields)
            {
                var field = (UserFieldsMD) company.GetBusinessObject(BoObjectTypes.oUserFields);
                var adapter = new UserFieldAdapter(field, userField);
                adapter.Execute();

                var addUserField = new AddUserField(company, field);
                addUserField.OnError += OnAddUserFieldError;
                addUserField.Execute();
            }
        }

        private void CreateObjectsFormXDocument(XDocument xml)
        {
            var xmlUserObjects = xml.Descendants(@"UserObject");

            foreach (XElement xmlUserObject in xmlUserObjects)
            {
                var userObject = (UserObjectsMD) company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
                var adapter = new UserObjectAdapter(userObject, xmlUserObject);
                adapter.Execute();

                var addUserObject = new AddUserObject(company, userObject);
                addUserObject.OnError += OnAddUserObjectError;
                addUserObject.Execute();
            }
        }

        private void CreateTablesFromXDocument(XDocument xml)
        {
            var userTables = xml.Descendants(@"UserTable");

            foreach (XElement userTable in userTables)
            {
                var table = (UserTablesMD) company.GetBusinessObject(BoObjectTypes.oUserTables);
                var adapter = new UserTableAdapter(table, userTable);
                adapter.Execute();
                var addUserTable = new AddUserTable(company, table);
                addUserTable.Error += OnAddUserTableError;
                addUserTable.Execute();
            }
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

        private string ReadFile(string fileName, Assembly assembly)
        {
            var reader = new ManifestResourceReader(assembly);
            return reader.ReadText(fileName);
        }
    }
}
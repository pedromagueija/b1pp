// <copyright filename="UserObjectAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Xml.Linq;
using SAPbobsCOM;

namespace B1PP.Database.Adapters
{
    /// <summary>
    /// Converts an xml representation of a UserObject into a UserObjectsMD object.
    /// </summary>
    internal class UserObjectAdapter : AdapterBase
    {
        private readonly XElement root;
        private readonly IUserObjectsMD userObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserObjectAdapter" /> class.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <param name="root">The XML user object.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="userObject" /> or <paramref name="root" /> are null.
        /// </exception>
        public UserObjectAdapter(IUserObjectsMD userObject, XElement root)
        {
            if (userObject == null)
            {
                throw new ArgumentNullException(nameof(userObject));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            this.userObject = userObject;
            this.root = root;
        }

        public void Execute()
        {
            var attributes = root.Attributes();

            PopulateProperties<IUserObjectsMD>(attributes, userObject);

            var findColumns = root.Descendants(@"FindColumn");
            PopulateCollection<IUserObjectMD_FindColumns>(findColumns, userObject.FindColumns);

            var childTables = root.Descendants(@"ChildTable");
            PopulateCollection<IUserObjectMD_ChildTables>(childTables, userObject.ChildTables);
        }
    }
}
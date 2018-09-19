// <copyright filename="UserFieldAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    using System.Xml.Linq;

    using SAPbobsCOM;

    internal class UserFieldAdapter : AdapterBase
    {
        private readonly XElement root;
        private readonly IUserFieldsMD userField;

        public UserFieldAdapter(IUserFieldsMD userField, XElement root)
        {
            this.userField = userField;
            this.root = root;
        }

        public void Execute()
        {
            var attributes = root.Attributes();
            PopulateProperties<IUserFieldsMD>(attributes, userField);

            var validValues = root.Descendants(@"ValidValue");
            PopulateCollection<IValidValuesMD>(validValues, userField.ValidValues);
        }
    }
}
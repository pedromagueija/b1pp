// <copyright filename="UserFieldAdapter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System.Xml.Linq;
using SAPbobsCOM;

namespace B1PP.Database.Adapters
{
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
// <copyright filename="SystemFormLoader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Extensions.SDK.UI;

    using SAPbouiCOM;

    internal class SystemFormLoader : FormLoader
    {
        private readonly object form;
        private readonly string formId;
        protected override IList<IFormPreProcessor> PreProcessors { get; }

        public SystemFormLoader(Application application, string formId, object form) : base(application)
        {
            this.formId = formId;
            this.form = form;

            PreProcessors = new List<IFormPreProcessor>
            {
                new CenterFormPreProcessor(application),
                new ImageBaseDirectoryPreProcessor()
            };
        }

        public override Form Load()
        {
            Assembly assembly = form.GetType().Assembly;
            string formType = $"{form.GetType().FullName}.xml";
            string contents = ReadFileContents(assembly, formType);

            XDocument xml = XDocument.Parse(contents);
            XElement formElement = xml.XPathSelectElement(@"//form");
            XAttribute idAttr = formElement.Attribute(@"uid");
            idAttr.Value = formId;

            UpdateForm(xml.ToString());

            return Application.Forms.Item(formId);
        }

        private void UpdateForm(string contents)
        {
            XDocument xml = PreProcess(contents);

            Application.Apply(xml.ToString());
        }
    }
}
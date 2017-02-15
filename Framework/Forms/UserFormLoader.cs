// <copyright filename="UserFormLoader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Linq;

    using Extensions.SDK.UI;

    using SAPbouiCOM;

    public class UserFormLoader : FormLoader
    {
        private readonly object form;
        protected override IList<IFormPreProcessor> PreProcessors { get; }

        public UserFormLoader(Application application, object form) : base(application)
        {
            this.form = form;
            PreProcessors = new List<IFormPreProcessor>
            {
                new CenterFormPreProcessor(application),
                new ImageBaseDirectoryPreProcessor()
            };
        }

        public override Form Load()
        {
            Type type = form.GetType();
            Assembly assembly = type.Assembly;
            string formType = type.FullName;
            string fileName = $"{formType}.xml";
            string contents = ReadFileContents(assembly, fileName);
            string uniqueId = GenerateUniqueId();

            return Load(contents, formType, uniqueId);
        }

        private Form CreateForm(string formType, string formId, string xmlData, BoFormModality modality = BoFormModality.fm_None)
        {
            var formDefinition = Application.Create<FormCreationParams>(BoCreatableObjectType.cot_FormCreationParams);
            formDefinition.XmlData = xmlData;
            formDefinition.UniqueID = formId;
            formDefinition.FormType = formType;
            formDefinition.Modality = modality;

            return Application.Forms.AddEx(formDefinition);
        }

        private string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private Form Load(string formContents, string formType, string formId)
        {
            XDocument formXml = PreProcess(formContents);

            return CreateForm(formType, formId, formXml.ToString());
        }
    }
}
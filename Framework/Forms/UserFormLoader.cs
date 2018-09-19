// <copyright filename="UserFormLoader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms
{
    using System;
    using System.Collections.Generic;

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
            var type = form.GetType();
            var assembly = type.Assembly;
            var formType = type.FullName;
            var fileName = $"{formType}.xml";
            var contents = ReadFileContents(assembly, fileName);
            var uniqueId = GenerateUniqueId();

            return Load(contents, formType, uniqueId);
        }

        private Form CreateForm(string formType, string formId, string xmlData,
            BoFormModality modality = BoFormModality.fm_None)
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
            var formXml = PreProcess(formContents);

            return CreateForm(formType, formId, formXml.ToString());
        }
    }
}
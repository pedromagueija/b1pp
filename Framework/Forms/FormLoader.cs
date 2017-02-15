// <copyright filename="FormLoader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using SAPbouiCOM;

    public abstract class FormLoader
    {
        protected readonly Application Application;
        protected abstract IList<IFormPreProcessor> PreProcessors { get; }

        protected FormLoader(Application application)
        {
            Application = application;
        }

        public abstract Form Load();

        protected XDocument PreProcess(string formContents)
        {
            XDocument formXml = XDocument.Parse(formContents);
            foreach (IFormPreProcessor preProcessor in PreProcessors)
            {
                preProcessor.Process(formXml);
            }
            return formXml;
        }

        protected string ReadFileContents(Assembly assembly, string fileName)
        {
            if (!ResourceExists(fileName, assembly))
            {
                string message =
                    $"The resource '{fileName}' was not found in assembly '{assembly}'." +
                    $"Consider making '{fileName}' an Embedded Resource.";

                throw new NotFoundException(message);
            }

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Failed to open a stream to '{fileName}'.");
                }

                return ReadContentsFromStream(stream);
            }
        }

        private string ReadContentsFromStream(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private bool ResourceExists(string fileName, Assembly assembly)
        {
            string resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.Equals(fileName));

            return !string.IsNullOrEmpty(resourceName);
        }
    }
}
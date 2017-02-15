// <copyright filename="DiApiConfigSettingHelper.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Connections
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class DiApiConfigSettingHelper
    {
        public string CreateConfigFile()
        {
            string path = Path.GetTempFileName();

            string contents = ReadText(@"diapi.credentials.xml");
            XDocument xml = XDocument.Parse(contents);
            if (xml.Root == null)
            {
                return string.Empty;
            }
            XElement passwordElement = xml.Root.XPathSelectElement(@"//Password");
            passwordElement.Value = Environment.GetEnvironmentVariable(@"B1_Password") ?? string.Empty;
            File.WriteAllText(path, xml.ToString());

            return path;
        }

        private string ReadText(string resourceName)
        {
            Assembly assembly = Assembly.GetAssembly(GetType());
            var resources = assembly.GetManifestResourceNames();
            string fullResourceName = resources.FirstOrDefault(r => r.EndsWith(resourceName));

            if(string.IsNullOrEmpty(fullResourceName))
                throw new ArgumentException($@"Invalid resource name {resourceName}.");

            using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
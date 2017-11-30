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
            var path = Path.GetTempFileName();

            var contents = ReadText(@"diapi.credentials.xml");
            var xml = XDocument.Parse(contents);
            if (xml.Root == null)
            {
                return string.Empty;
            }
            var passwordElement = xml.Root.XPathSelectElement(@"//Password");
            passwordElement.Value = Environment.GetEnvironmentVariable(@"B1_Password") ?? string.Empty;
            File.WriteAllText(path, xml.ToString());

            return path;
        }

        private string ReadText(string resourceName)
        {
            var assembly = Assembly.GetAssembly(GetType());
            var resources = assembly.GetManifestResourceNames();
            var fullResourceName = resources.FirstOrDefault(r => r.EndsWith(resourceName));

            if(string.IsNullOrEmpty(fullResourceName))
                throw new ArgumentException($@"Invalid resource name {resourceName}.");

            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
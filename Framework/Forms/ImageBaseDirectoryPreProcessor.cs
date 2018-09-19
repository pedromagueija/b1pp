// <copyright filename="ImageBaseDirectoryPreProcessor.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class ImageBaseDirectoryPreProcessor : IFormPreProcessor
    {
        public void Process(XDocument formXml)
        {
            var baseDirectory = Environment.CurrentDirectory;
            var icons = formXml.XPathSelectElements(@"//*[@Image != '']");

            foreach (var icon in icons)
            {
                var imageAttr = icon.Attribute(@"Image");
                imageAttr.Value = Path.Combine(baseDirectory, imageAttr.Value);
            }
        }
    }
}
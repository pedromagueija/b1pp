// <copyright filename="IFormPreProcessor.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms
{
    using System.Xml.Linq;

    public interface IFormPreProcessor
    {
        void Process(XDocument formXml);
    }
}
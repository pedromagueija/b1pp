// <copyright filename="CenterFormPreProcessor.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms
{
    using System.Globalization;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using SAPbouiCOM;

    internal class CenterFormPreProcessor : IFormPreProcessor
    {
        private readonly Application application;

        public CenterFormPreProcessor(Application application)
        {
            this.application = application;
        }

        public void Process(XDocument formXml)
        {
            XElement formElement = formXml.XPathSelectElement(@"//form");

            XAttribute heightAttr = formElement.Attribute(@"height");
            XAttribute widthAttr = formElement.Attribute(@"width");
            XAttribute leftAttr = formElement.Attribute(@"left");
            XAttribute topAttr = formElement.Attribute(@"top");

            if (heightAttr == null || widthAttr == null || leftAttr == null || topAttr == null)
            {
                return;
            }

            int formHeight;
            int formWidth;

            bool isHeight = int.TryParse(heightAttr.Value, out formHeight);
            bool isWidth = int.TryParse(widthAttr.Value, out formWidth);

            if (!isHeight || !isWidth)
            {
                return;
            }

            int top = application.Desktop.Height / 2 - formHeight / 2;
            int left = application.Desktop.Width / 2 - formWidth / 2;

            leftAttr.Value = left.ToString(CultureInfo.InvariantCulture);
            topAttr.Value = top.ToString(CultureInfo.InvariantCulture);
        }
    }
}
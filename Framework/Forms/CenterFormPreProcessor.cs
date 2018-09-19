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
            var formElement = formXml.XPathSelectElement(@"//form");

            var heightAttr = formElement.Attribute(@"height");
            var widthAttr = formElement.Attribute(@"width");
            var leftAttr = formElement.Attribute(@"left");
            var topAttr = formElement.Attribute(@"top");

            if (heightAttr == null || widthAttr == null || leftAttr == null || topAttr == null)
            {
                return;
            }

            int formHeight;
            int formWidth;

            var isHeight = int.TryParse(heightAttr.Value, out formHeight);
            var isWidth = int.TryParse(widthAttr.Value, out formWidth);

            if (!isHeight || !isWidth)
            {
                return;
            }

            var top = application.Desktop.Height / 2 - formHeight / 2;
            var left = application.Desktop.Width / 2 - formWidth / 2;

            leftAttr.Value = left.ToString(CultureInfo.InvariantCulture);
            topAttr.Value = top.ToString(CultureInfo.InvariantCulture);
        }
    }
}
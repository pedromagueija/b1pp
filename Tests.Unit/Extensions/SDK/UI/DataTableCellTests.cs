// <copyright filename="DataTableCellTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

// ReSharper disable ObjectCreationAsStatement
namespace Tests.Unit.Extensions.SDK.UI
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    using B1PP.Extensions.SDK.UI;

    using NUnit.Framework;

    internal class DataTableCellTests
    {
        [Test]
        public void ShouldCreateXmlCellWithGivenData()
        {
            var sb = new StringBuilder();
            var sut = new DataTableCell(@"id", @"value");
            var writer = new XmlTextWriter(new StringWriter(sb));

            sut.WriteXml(writer);

            Assert.AreEqual($@"<Cell><ColumnUid>id</ColumnUid><Value>value</Value></Cell>", sb.ToString());
        }

        [Test]
        [TestCase(@"id", null, @"")]
        [TestCase(@"id", @"", @"")]
        public void ShouldCreateXmlCellWithEmptyData(string id, string value, string expected)
        {
            var sb = new StringBuilder();
            var sut = new DataTableCell(id, value);
            var writer = new XmlTextWriter(new StringWriter(sb));

            sut.WriteXml(writer);

            Assert.AreEqual($@"<Cell><ColumnUid>{id}</ColumnUid><Value /></Cell>", sb.ToString());
        }

        [Test]
        [TestCase(null)]
        [TestCase(@"")]
        public void ShouldThrowExceptionForInvalidColumnId(string id)
        {
            Assert.Throws<ArgumentException>(() => new DataTableCell(id, @"columnValue"));
        }
    }
}
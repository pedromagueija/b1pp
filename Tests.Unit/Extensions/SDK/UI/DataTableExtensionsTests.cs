// <copyright filename="DataTableExtensionsTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Extensions.SDK.UI
{
    using B1PP.Extensions.SDK.UI;

    using NSubstitute;

    using NUnit.Framework;

    internal class DataTableExtensionsTests
    {
        [Test]
        public void AddValueAddsRowWhenDataTableIsEmpty()
        {
            var fake = Substitute.For<SAPbouiCOM.DataTable>();

            DataTableExtensions.AddValue(fake, new TestClass());

            fake.Received().Rows.Add();
        }

        [Test]
        public void AddValueUsesLastRowWhenDataTableIsNotEmpty()
        {
            var fake = Substitute.For<SAPbouiCOM.DataTable>();
            fake.Rows.Count.Returns(1);

            DataTableExtensions.AddValue(fake, new TestClass());

            fake.Rows.DidNotReceive().Add();
        }
    }

    internal class TestClass
    {
        public string Property { get; set; }
    }
}
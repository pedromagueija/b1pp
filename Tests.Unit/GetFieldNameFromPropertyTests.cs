// <copyright filename="GetFieldNameFromPropertyTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP;
using B1PP.Database.Attributes;
using NUnit.Framework;

namespace Tests.Unit
{
    public class GetFieldNameFromPropertyTests
    {
        [Test]
        public void System_No_Custom_Name_Returns_Property_Name()
        {
            var docEntryProperty = new TestClass().GetType().GetProperty(@"DocEntry");
            var getFieldName = new GetFieldName();

            string name = getFieldName.FromProperty(docEntryProperty);

            Assert.AreEqual(@"DocEntry", name);
        }

        public class TestClass
        {
            [SystemField]
            public string DocEntry { get; set; }
        }
    }
}
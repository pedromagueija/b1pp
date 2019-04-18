// <copyright filename="GetFieldNameFromPropertyTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP;
using B1PP.Database;
using B1PP.Database.Attributes;
using NUnit.Framework;
using SAPbobsCOM;

namespace Tests.Unit
{
    public class GetFieldNameFromPropertyTests
    {
        [Test]
        [TestCase(@"DocEntry", @"DocEntry")]
        [TestCase(@"DocumentName", @"DocName")]
        [TestCase(@"UserEntry", @"U_UserEntry")]
        [TestCase(@"UserName", @"U_UName")]
        public void Should_Correctly_Determine_Name(string propertyName, string expectedFieldName)
        {
            var property = new TestClass().GetType().GetProperty(propertyName);
            var getFieldName = new GetFieldName();

            string name = getFieldName.FromProperty(property);

            Assert.AreEqual(expectedFieldName, name);
        }

        public class TestClass
        {
            [SystemField]
            public string DocEntry { get; set; }
            
            [SystemField]
            [FieldName(@"DocName")]
            public string DocumentName { get; set; }

            [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
            public string UserEntry { get; set; }
            
            [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
            [FieldName(@"UName")]
            public string UserName { get; set; }

        }
    }
}
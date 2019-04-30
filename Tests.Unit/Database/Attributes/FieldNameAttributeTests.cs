// <copyright filename="FieldNameAttributeTests.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP.Database.Attributes;
using Castle.Core.Internal;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SAPbobsCOM;

namespace Tests.Unit.Database.Attributes
{
    public class FieldNameAttributeTests
    {
        [Test]
        [TestCase(@"TaxId")]
        [TestCase(@"Address")]
        public void Determines_Field_Name_Correctly(string fieldName)
        {
            var field = Substitute.For<UserFieldsMD>();
            var property = typeof(TestClass).GetProperty(fieldName);
            var attr = property.GetAttribute<FieldNameAttribute>();
            
            attr.Apply(field, property);
            
            property.Name.Should().Be(fieldName);
        }

        private class TestClass
        {
            [FieldName(@"U_TaxId")]
            public string TaxId { get; set; }

            [FieldName("Address")]
            public string Address { get; set; }
        }
    }
}
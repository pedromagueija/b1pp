// <copyright filename="EnumConverterTests.cs" project="B1PP.DI.Schema.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Database
{
    using System;
    using System.Collections.Generic;

    using B1PP.Database;

    using NUnit.Framework;

    internal class EnumConverterTests
    {


        [Test]
        public void CanConvertEnumToEnumerable()
        {
            var converter = new EnumConverter(typeof(TestEnum));
            var values = converter.ToEnumerable();
            var expected = new List<Tuple<string, string>>
            {
                new Tuple<string, string>(@"1", @"One"),
                new Tuple<string, string>(@"2", @"Two"),
                new Tuple<string, string>(@"3", @"Three")
            };


            Assert.IsNotEmpty(values);            
            Assert.AreEqual(expected, values);
        }

        private enum TestEnum
        {
            One = 1,
            Two,
            Three
        }
    }
}
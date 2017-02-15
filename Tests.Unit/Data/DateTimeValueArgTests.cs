// <copyright filename="DateTimeValueArgTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Data
{
    using System;

    using B1PP.Data;

    using NUnit.Framework;

    internal class DateTimeValueArgTests
    {
        [Test]
        public void CanCreateDateTimeValueArg()
        {
            Assert.NotNull(new DateTimeValueArg("@placeHolder", DateTime.Today));
        }


        [Test]
        public void ConvertsDateToUniversalSqlDateTimeFormat()
        {
            var arg = new DateTimeValueArg("@placeHolder", new DateTime(2013, 6, 30));
            Assert.AreEqual("'20130630'", arg.Value);
        }
    }
}
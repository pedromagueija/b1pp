// <copyright filename="IdTests.cs" project="B1PP.Extensions.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.Tests.Unit.Types
{
    using Extensions.Types;

    using NUnit.Framework;

    internal class IdTests
    {


        [Test]
        public void CanConvertValidIntToInvariantString()
        {
            string s = new Id(12345);

            Assert.AreEqual(@"12345", s);
        }



        [Test]
        public void CanConvertNullIdToEmptyString()
        {
            string s = (Id) null;

            Assert.IsEmpty(s);
        }


    }
}
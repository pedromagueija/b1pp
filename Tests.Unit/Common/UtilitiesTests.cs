// <copyright filename="UtilitiesTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Common
{
    using B1PP.Common;

    using NUnit.Framework;

    internal class UtilitiesTests
    {

        [Test]
        public void AttemptingToReleaseNonComObjectDoesNotThrow()
        {
            var obj = new object();

            Assert.DoesNotThrow(() => Utilities.Release(obj));
        }


    }
}
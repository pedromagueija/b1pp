// <copyright filename="StatusBarMessageTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Forms
{
    using System;

    using B1PP.Forms;

    using NUnit.Framework;

    internal class StatusBarMessageTests
    {

        [Test]
        public void ThrowsArgumentExceptionWhenApplicationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new StatusBarMessage(null, "text", new object()));
        }


    }
}
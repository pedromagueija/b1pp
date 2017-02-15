// <copyright filename="UiApiConnectionTests.cs" project="B1PP.Connections.Tests.Features">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Connections
{
    using B1PP.Connections;

    using NUnit.Framework;

    [TestFixture]
    public class UiApiConnectionTests
    {
        [Test]
        public void ConnectShouldAllowAccessToApplication()
        {
            var connection = new UiApiConnection();

            connection.Connect();

            Assert.That(connection.Application, Is.Not.Null);
        }

        [Test]
        public void DisconnectShouldDisconnectFromCompany()
        {
            var connection = new UiApiConnection();

            connection.Disconnect();

            Assert.IsNull(connection.Application);
        }
    }
}
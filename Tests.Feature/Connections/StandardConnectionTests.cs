// <copyright filename="StandardConnectionTests.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Connections
{
    using B1PP.Connections;

    using NUnit.Framework;

    [TestFixture]
    public class StandardConnectionTests
    {
        [Test]
        public void CompanyAndApplicationAreNullBeforeConnect()
        {
            var connection = new StandardConnection();

            Assert.IsNull(connection.Application);
            Assert.IsNull(connection.Company);
        }

        [Test]
        public void ConnectAllowsAccessToCompanyAndApplication()
        {
            var connection = new StandardConnection();

            connection.Connect();

            Assert.IsNotNull(connection.Application);
            Assert.IsNotNull(connection.Company);
        }

        [Test]
        public void DisconnectDisconnectsCompanyAndApplication()
        {
            var connection = new StandardConnection();
            connection.Connect();

            connection.Disconnect();

            Assert.IsNull(connection.Application);
            Assert.IsNull(connection.Company);
        }
    }
}
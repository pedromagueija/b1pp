// <copyright filename="DiApiConnectionTests.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Connections
{
    using B1PP.Connections;

    using NUnit.Framework;

    [TestFixture]
    public class DiApiConnectionTests
    {
        private readonly DiApiConfigSettingHelper config = new DiApiConfigSettingHelper();

        [Test]
        public void CompanyIsNullAfterDisconnect()
        {
            var configFile = config.CreateConfigFile();
            var settings = DiApiConnectionSettings.Load(configFile);
            var connection = new DiApiConnection(settings);
            connection.Connect();

            connection.Disconnect();

            Assert.IsNull(connection.Company);
        }

        [Test]
        public void CompanyIsNullBeforeConnect()
        {
            var configFile = config.CreateConfigFile();
            var settings = DiApiConnectionSettings.Load(configFile);
            var connection = new DiApiConnection(settings);

            Assert.IsNull(connection.Company);
        }

        [Test]
        public void ConnectAllowsAccessToCompany()
        {
            var configFile = config.CreateConfigFile();
            var settings = DiApiConnectionSettings.Load(configFile);
            var connection = new DiApiConnection(settings);

            connection.Connect();

            Assert.IsNotNull(connection);
            Assert.IsNotNull(connection.Company);
        }

        [Test]
        public void NullSettingsThrowsConnectionException()
        {
            var connection = new DiApiConnection(null);

            Assert.Throws<ConnectionException>(() => connection.Connect());
        }
    }
}
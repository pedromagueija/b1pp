// <copyright filename="ConnectionStringTests.cs" project="B1PP.Connections.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Connections
{
    using B1PP.Connections;

    using NUnit.Framework;

    [TestFixture]
    public class ConnectionStringTests
    {
        [Test]
        public void CanCreateB1ConnectionString()
        {
            string[] args = {};
            Assert.NotNull(new ConnectionString(args));
        }

        [Test]
        public void ReturnsB1ConnectionStringWhenAddon()
        {
            var sampleConnectionString = "ConnectionStringMustBeAtLeast20CharsLong";
            string[] args = {"Addon.exe", sampleConnectionString};
            var connectionString = new ConnectionString(args).GetConnectionString();

            Assert.That(connectionString, Is.EqualTo(sampleConnectionString));
        }

        [Test]
        public void ReturnsDevConnectionStringWhenDevEnv()
        {
            string[] args = {"devenv.exe", "OTHERARGS"};
            var connectionString = new ConnectionString(args).GetConnectionString();

            var devConnString =
                "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
            Assert.That(connectionString, Is.EqualTo(devConnString));
        }
    }
}
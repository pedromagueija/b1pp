// <copyright filename="InsistentStandardConnectionTests.cs" project="B1PP.Connections.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Connections
{
    using B1PP.Connections;

    using NSubstitute;

    using NUnit.Framework;

    internal class InsistentStandardConnectionTests
    {
        [Test]
        public void AttemptsToConnectMultipleTimesOnError()
        {
            var connection = Substitute.For<IConnection>();
            connection.When(c => c.Connect()).Throw<ConnectionException>();

            var sut = new InsistentConnection(connection);

            try
            {
                sut.Connect();
            }
            catch (ConnectionException)
            {
                // expected to throw connection
            }

            Assert.AreEqual(false, sut.Connected);
            Assert.AreEqual(3, sut.Audit.Messages.Count);
        }

        [Test]
        public void AttemptsToConnectOnceOnSuccess()
        {
            var connection = Substitute.For<IConnection>();
            connection.Connected.Returns(true);

            var sut = new InsistentConnection(connection);

            sut.Connect();

            Assert.AreEqual(true, sut.Connected);
            Assert.AreEqual(0, sut.Audit.Messages.Count);
        }


        [Test]
        public void WhenConnectFailsThrowsConnectionException()
        {
            var connection = Substitute.For<IConnection>();
            connection.When(c => c.Connect()).Throw<ConnectionException>();

            var sut = new InsistentConnection(connection);

            Assert.Throws<ConnectionException>(() => sut.Connect());
        }

        [Test]
        public void WhenConnectSucceedsDoesNotThrowConnectionException()
        {
            var connection = Substitute.For<IConnection>();
            connection.Connected.Returns(true);

            var sut = new InsistentConnection(connection);

            Assert.DoesNotThrow(() => sut.Connect());
        }
    }
}
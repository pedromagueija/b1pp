// <copyright filename="ApplicationEventsHandlerBaseTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Forms.Events.ApplicationEvents
{
    using B1PP;
    using B1PP.Forms.Events.ApplicationEvents;

    using NSubstitute;

    using NUnit.Framework;

    internal class ApplicationEventsHandlerBaseTests
    {
        private ApplicationEventsHandlerBase handler;
        private IAddon fakeAddon;

        [SetUp]
        public void Setup()
        {
            fakeAddon = Substitute.For<IAddon>();
            handler = new DefaultApplicationEventsHandler(fakeAddon);
        }

        [Test]
        public void OnAddonStopped_Exits_Addon()
        {
            handler.OnAddonStopped();

            fakeAddon.Received(1).Exit();
        }

        [Test]
        public void OnShutdown_Exits_Addon()
        {
            handler.OnShutdown();

            fakeAddon.Received(1).Exit();
        }

        [Test]
        public void OnCompanyChanged_Restarts_Addon()
        {
            handler.OnCompanyChanged();

            fakeAddon.Received(1).Restart();
        }
    }
}
// <copyright filename="StandardAddonBase.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP
{
    using System;

    using Connections;

    using Forms;
    using Forms.Events;
    using Forms.Events.ApplicationEvents;

    /// <summary>
    /// Standard UI and DI API add-on.
    /// </summary>
    public abstract class StandardAddonBase : IAddon
    {
        private IApplicationEventsHandler appEventsHandler;
        protected IConnection connection;
        private B1EventsManager events;

        public virtual void Start()
        {
            Connect();

            //SetupSchema();
            //SetupMainMenu();
            //SetupEvents();

            B1EventsManager.Instance.Initialize(connection.Application, GetApplicationEventHandler());

            var status = new StatusBarMessage(
                connection.Application,
                $@"Addon ready at {connection.Application.Company.Name}.");
            status.Warning();
        }

        public void Stop()
        {
            events.Terminate();
            //mainMenu.Terminate();
            //B1.Disconnect();

            connection.Disconnect();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Exit()
        {
            Stop();
            Environment.Exit(0);
        }

        protected void SetApplicationEventsHandler(IApplicationEventsHandler handler)
        {
            appEventsHandler = handler ?? new DefaultApplicationEventsHandler(this);
        }

        private void Connect()
        {
            var connectionFactory = new ConnectionFactory();
            connection = connectionFactory.CreateStandardConnection();

            connection.Connect();
        }

        private IApplicationEventsHandler GetApplicationEventHandler()
        {
            return appEventsHandler ?? new DefaultApplicationEventsHandler(this);
        }
    }
}
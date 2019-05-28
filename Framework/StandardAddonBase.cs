// <copyright filename="StandardAddonBase.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP.Forms.Events.MenuEvents;

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
        protected IMainMenuInstance MainMenu;
        protected IStandardConnection connection;
        private B1EventsManager events;

        public virtual void Start()
        {
            Connect();

            B1EventsManager.Instance.Initialize(connection.Application, GetApplicationEventHandler(), MainMenu);

            ShowReady();
        }

        private void ShowReady()
        {
            var app = connection.Application;
            string companyName = app.Company.Name;
            
            var status = new StatusBarMessage(app, $@"Add-on ready at {companyName}.");
            status.Warning();
        }

        public virtual void Stop()
        {
            events.Terminate();

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
            connection = ConnectionFactory.CreateStandardConnection();
            connection.Connect();
        }

        private IApplicationEventsHandler GetApplicationEventHandler()
        {
            return appEventsHandler ?? new DefaultApplicationEventsHandler(this);
        }
    }
}
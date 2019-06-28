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
    using Forms.Events.MenuEvents;

    /// <summary>
    /// Standard UI and DI API add-on.
    /// </summary>
    public abstract class StandardAddonBase : IAddon
    {
        /// <summary>
        /// Current connection to Business One.
        /// </summary>
        protected IStandardConnection Connection { get; private set; }
        private IApplicationEventsHandler appEventsHandler;
        private IMainMenuInstance mainMenu = new DefaultMainMenu();

        /// <summary>
        /// When true, no message will be displayed after add-on connection.
        /// </summary>
        protected bool SuppressReadyMessage { get; set; }

        /// <summary>
        /// Stops the add-on and terminates the process.
        /// </summary>
        public void Exit()
        {
            Stop();
            Environment.Exit(0);
        }

        /// <summary>
        /// Restarts the add-on without terminating the process.
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Starts the add-on.
        /// </summary>
        public virtual void Start()
        {
            Connect();

            B1EventsManager.Instance.Initialize(Connection.Application, GetApplicationEventHandler());

            ShowReady();
        }


        /// <summary>
        /// Stops the add-on without terminating the process.
        /// </summary>
        public virtual void Stop()
        {
            B1EventsManager.Instance.Terminate();
            Connection.Disconnect();
        }

        /// <summary>
        /// Set a custom built application event handler that will be responsible to handle application events
        /// such as Shutdown or LanguageChanged.
        /// </summary>
        /// <param name="handler">
        /// The handler to use.
        /// </param>
        protected void SetApplicationEventsHandler(IApplicationEventsHandler handler)
        {
            appEventsHandler = handler ?? new DefaultApplicationEventsHandler(this);
        }

        /// <summary>
        /// Set a custom built main menu instance.
        /// </summary>
        /// <param name="instance">
        /// The instance to use.
        /// </param>
        protected void SetMainMenu(IMainMenuInstance instance)
        {
            mainMenu = instance ?? new DefaultMainMenu();

            B1EventsManager.Instance.SetMainMenu(mainMenu);
        }

        private void Connect()
        {
            Connection = ConnectionFactory.CreateStandardConnection();
            Connection.Connect();
        }

        private IApplicationEventsHandler GetApplicationEventHandler()
        {
            return appEventsHandler ?? new DefaultApplicationEventsHandler(this);
        }

        private void ShowReady()
        {
            if (SuppressReadyMessage)
            {
                return;
            }

            var app = Connection.Application;
            string companyName = app.Company.Name;

            var status = new StatusBarMessage(app, $@"Add-on ready at {companyName}.");
            status.Warning();
        }
    }
}
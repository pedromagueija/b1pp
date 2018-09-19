// <copyright filename="StandardConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    using System;
    using System.Runtime.InteropServices;

    using Common;

    using SAPbouiCOM;

    using DiCompany = SAPbobsCOM.Company;
    using DiCompanyClass = SAPbobsCOM.CompanyClass;

    /// <summary>
    /// Provides access to the Application and Company objects from Business One
    /// <para />
    /// by establishing a connection to the API.
    /// <para />
    /// When disconnected the Application and Company objects will be null.
    /// </summary>
    internal class StandardConnection : IConnection
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        private readonly ConnectionString connectionString;

        /// <summary>
        /// Gets the application.
        /// </summary>
        public Application Application { get; private set; }

        /// <summary>
        /// Gets the company.
        /// </summary>
        public DiCompany Company { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IConnection" /> is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardConnection" /> class.
        /// </summary>
        public StandardConnection()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            connectionString = new ConnectionString(commandLineArgs);
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <exception cref="ConnectionException">
        /// Thrown when an error connecting to Business One occurs.
        /// </exception>
        public virtual void Connect()
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                var gui = ConnectToGui();
                ConnectToCompany(gui);

                Connected = true;
            }
            catch (COMException e)
            {
                throw ConnectionException.CreateFrom(e);
            }
        }

        /// <summary>
        /// Disconnects from SAP Business One.
        /// </summary>
        public void Disconnect()
        {
            if (!Connected)
            {
                return;
            }

            if (Company != null)
            {
                Company.Disconnect();
                Utilities.Release(Company);
                Company = null;
            }

            if (Application != null)
            {
                Utilities.Release(Application);
                Application = null;
            }

            Connected = false;
        }

        private void ConnectToCompany(ISboGuiApi gui)
        {
            Application = gui.GetApplication();
            Company = new DiCompanyClass {Application = Application};

            int result = Company.Connect();
            if (result != 0)
            {
                string lastErrorDescription = Company.GetLastErrorDescription();
                string message = $"{result} {lastErrorDescription}";
                throw new ConnectionException(message);
            }
        }

        private SboGuiApiClass ConnectToGui()
        {
            var gui = new SboGuiApiClass();
            gui.Connect(connectionString);
            return gui;
        }
    }
}
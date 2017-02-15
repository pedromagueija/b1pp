// <copyright filename="UiApiConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbouiCOM;

    using DiCompany = SAPbobsCOM.Company;

    /// <summary>
    /// Provides a connection to the UI API only.
    /// </summary>
    internal class UiApiConnection : IConnection
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
        /// <remarks>
        /// Relevant only on connection that support the Company, null otherwise.
        /// </remarks>
        public DiCompany Company { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IConnection" /> is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiApiConnection" /> class.
        /// </summary>
        public UiApiConnection()
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
        public void Connect()
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                var gui = new SboGuiApi();
                gui.Connect(connectionString);
                Application = gui.GetApplication();
            }
            catch (COMException e)
            {
                throw ConnectionException.CreateFrom(e);
            }

            Connected = true;
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

            if (Application != null)
            {
                Utilities.Release(Application);
                Application = null;
            }

            Connected = false;
        }
    }
}
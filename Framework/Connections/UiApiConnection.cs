// <copyright filename="UiApiConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    using System;
    using System.Runtime.InteropServices;

    using Common;

    using Exceptions;

    using SAPbouiCOM;

    using DiCompany = SAPbobsCOM.Company;

    internal class UiApiConnection : IUiApiConnection
    {
        private readonly ConnectionString connectionString;

        public Application Application { get; private set; }

        public bool Connected { get; private set; }

        public UiApiConnection()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            connectionString = new ConnectionString(commandLineArgs);
        }

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
                throw ConnectionFailedException.CreateFrom(e);
            }

            Connected = true;
        }

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
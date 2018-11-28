// <copyright filename="StandardConnection.cs" project="Framework">
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
    using DiCompanyClass = SAPbobsCOM.CompanyClass;

    internal class StandardConnection : IStandardConnection
    {
        private readonly ConnectionString connectionString;

        public Application Application { get; private set; }

        public DiCompany Company { get; private set; }

        public bool Connected { get; private set; }

        public StandardConnection()
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

                var gui = ConnectToGui();
                ConnectToCompany(gui);

                Connected = true;
            }
            catch (COMException e)
            {
                throw ConnectionFailedException.CreateFrom(e);
            }
        }

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
                throw new ConnectionFailedException(message);
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
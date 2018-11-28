// <copyright filename="DiApiConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    using Common;

    using Exceptions;

    using SAPbouiCOM;

    using DiCompany = SAPbobsCOM.Company;

    /// <summary>
    /// Provides a connection to the DI API only.
    /// </summary>
    internal class DiApiConnection : IDiApiConnection
    {
        private readonly DiApiConnectionSettings settings;

        public DiCompany Company { get; private set; }

        public bool Connected { get; private set; }

        public DiApiConnection(DiApiConnectionSettings settings)
        {
            this.settings = settings ?? DiApiConnectionSettings.CreateEmptySettings();
        }

        public void Connect()
        {
            if (Connected)
            {
                return;
            }

            Company = settings.ToCompany();

            int result = Company.Connect();
            if (result != 0)
            {
                throw new ConnectionFailedException($"{result} {Company.GetLastErrorDescription()}");
            }

            Connected = true;
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

            Connected = false;
        }
    }
}
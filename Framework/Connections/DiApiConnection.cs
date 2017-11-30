// <copyright filename="DiApiConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using Common;

    using SAPbouiCOM;

    using DiCompany = SAPbobsCOM.Company;

    /// <summary>
    /// Provides a connection to the DI API only.
    /// </summary>
    internal class DiApiConnection : IConnection
    {
        /// <summary>
        /// The connection settings.
        /// </summary>
        private readonly DiApiConnectionSettings settings;

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <remarks>
        /// Relevant only on connection that support the Application, null otherwise.
        /// </remarks>
        public Application Application { get; } = null;

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

        public DiApiConnection(DiApiConnectionSettings settings)
        {
            this.settings = settings ?? DiApiConnectionSettings.CreateEmptySettings();
        }

        /// <summary>
        /// Connects to SAP Business One.
        /// </summary>
        /// <exception cref="ConnectionException">
        /// Thrown when a connection cannot be established.
        /// </exception>
        public void Connect()
        {
            if (Connected)
                return;

            Company = settings.ToCompany();

            var result = Company.Connect();
            if (result != 0)
            {
                var message = $"{result} {Company.GetLastErrorDescription()}";
                throw new ConnectionException(message);
            }

            Connected = true;
        }

        /// <summary>
        /// Disconnects from SAP Business One.
        /// </summary>
        public void Disconnect()
        {
            if (!Connected)
                return;

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
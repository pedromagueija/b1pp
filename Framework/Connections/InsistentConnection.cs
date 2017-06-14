// <copyright filename="InsistentConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using System;
    using System.Threading;

    using JetBrains.Annotations;

    using SAPbouiCOM;

    using Company = SAPbobsCOM.Company;

    /// <summary>
    /// Extends a connection with retry capabilities.
    /// </summary>
    /// <seealso cref="B1PP.Connections.IConnection" />
    internal class InsistentConnection : IConnection
    {
        /// <summary>
        /// The actual connection.
        /// </summary>
        private readonly IConnection connection;

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        public ActionResult Audit { get; } = new ActionResult();

        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Application" /> object, or null when the connection
        /// <para />
        /// type does not support it (e.g.: DI API only connections).
        /// </summary>
        public Application Application
        {
            get
            {
                return connection.Application;
            }
        }

        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Company" /> object, or null when the connection
        /// <para />
        /// type does not support it (e.g.: UI API only connections).
        /// </summary>
        public Company Company
        {
            get
            {
                return connection.Company;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IConnection" /> is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected
        {
            get
            {
                return connection.Connected;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsistentConnection" /> class.
        /// </summary>
        /// <remarks>
        /// By default the <see cref="InsistentConnection" /> will use a <see cref="StandardConnection" /> as its connection.
        /// </remarks>
        public InsistentConnection() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsistentConnection" /> class.
        /// </summary>
        /// <param name="connection">The connection type.</param>
        /// <remarks>
        /// By default the <see cref="InsistentConnection" /> will use a <see cref="StandardConnection" /> as its connection.
        /// </remarks>
        /// ///
        public InsistentConnection([CanBeNull] IConnection connection)
        {
            this.connection = connection ?? new StandardConnection();
        }

        /// <summary>
        /// Establishes the connection to SAP Business One.
        /// <para />
        /// Note that without connection both <see cref="Application" /> and <see cref="Company" /> are <see langword="null" />.
        /// </summary>
        /// <exception cref="ConnectionException">
        /// Thrown when an error occurs while connecting.
        /// </exception>
        public void Connect()
        {
            int attempts = 1;
            const int maxAttempts = 3;
            const int intervalMilliseconds = 100;

            do
            {
                ConnectAndAudit();

                if (Connected)
                {
                    return;
                }

                // wait for next attempt
                Thread.Sleep(attempts * intervalMilliseconds);
                attempts++;
            } while (attempts <= maxAttempts);

            // could not connect
            string message = $@"{Audit.LastMessage}{Environment.NewLine}" +
                             @"More information in the 'Data' property of the exception.";

            var e = new ConnectionException(message);
            e.Data.Add(@"errors", Audit.Messages);

            throw e;
        }

        /// <summary>
        /// Disconnects from SAP Business One.
        /// <para />
        /// Note that without after disconnecting both <see cref="Application" /> and <see cref="Company" /> are
        /// <see langword="null" />.
        /// </summary>
        public void Disconnect()
        {
            connection.Disconnect();
        }

        /// <summary>
        /// Attempts to connect and audits any failures.
        /// </summary>
        private void ConnectAndAudit()
        {
            try
            {
                connection.Connect();
            }
            catch (ConnectionException e)
            {
                Audit.Add(e.Message);
            }
        }
    }
}
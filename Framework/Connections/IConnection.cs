// <copyright filename="IConnection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using SAPbouiCOM;

    using Company = SAPbobsCOM.Company;

    /// <summary>
    /// Represents a connection to SAP Business One.
    /// A connection can be obtained from the <see cref="ConnectionFactory"/>.
    /// </summary>
    /// <example>
    /// How to perform a standard (UI and DI API) connection to SAP Business One.
    /// <code>
    /// IConnection connection = ConnectionFactory.CreateStandardConnection();
    /// connection.Connect();
    /// // Your code
    /// connection.Disconnect(); 
    /// </code>
    /// </example>
    public interface IConnection
    {
        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Application"/> object, or null when the connection<para/>
        /// type does not support it (e.g.: DI API only connections).
        /// </summary>
        Application Application { get; }

        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Company"/> object, or null when the connection<para/>
        /// type does not support it (e.g.: UI API only connections).
        /// </summary>
        Company Company { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IConnection"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        bool Connected { get; }

        /// <summary>
        /// Establishes the connection to SAP Business One.<para/>
        /// Note that without connection both <see cref="Application"/> and <see cref="Company"/> are <c>null</c>.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from SAP Business One.<para/>
        /// Note that without after disconnecting both <see cref="Application"/> and <see cref="Company"/> are <c>null</c>.
        /// </summary>
        void Disconnect();
    }
}
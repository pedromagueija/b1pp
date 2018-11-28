namespace B1PP.Connections
{
    using Exceptions;

    using SAPbobsCOM;

    /// <summary>
    /// Represents a DI API only connection.
    /// </summary>
    public interface IDiApiConnection
    {
        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Company" /> object.
        /// </summary>
        Company Company { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Establishes the connection.
        /// </summary>
        /// <exception cref="ConnectionFailedException">
        /// Thrown when the connection fails.
        /// </exception>
        void Connect();

        /// <summary>
        /// Terminates this connection.
        /// </summary>
        void Disconnect();
    }
}
namespace B1PP.Connections
{
    using Exceptions;

    using SAPbouiCOM;

    /// <summary>
    /// Represents a UI API connection.
    /// </summary>
    public interface IUiApiConnection
    {
        /// <summary>
        /// Returns the <see cref="SAPbouiCOM.Application" /> object.
        /// </summary>
        Application Application { get; }

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
// <copyright filename="ConnectionFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    /// <summary>
    /// Creates different types of connections.
    /// It does not connect or manage the lifetime of the connections.
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// Creates the DI API connection.
        /// </summary>
        /// <param name="settings">The DI API connection settings.</param>
        /// <returns>
        /// A new DI API connection.
        /// </returns>
        public IConnection CreateDiApiConnection(DiApiConnectionSettings settings)
        {
            return new InsistentConnection(new DiApiConnection(settings));
        }

        /// <summary>
        /// Creates the standard connection.
        /// </summary>
        /// <returns>
        /// A new standard (UI and DI API) connection.
        /// </returns>
        public IConnection CreateStandardConnection()
        {
            return new InsistentConnection(new StandardConnection());
        }

        /// <summary>
        /// Creates the UI API connection.
        /// </summary>
        /// <returns>
        /// A new UI API connection.
        /// </returns>
        public IConnection CreateUiApiConnection()
        {
            return new InsistentConnection(new UiApiConnection());
        }
    }
}
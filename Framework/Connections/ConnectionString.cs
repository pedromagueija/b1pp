// <copyright filename="ConnectionString.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the connection string used to connect to Business One.
    /// </summary>
    internal class ConnectionString
    {
        /// <summary>
        /// Gets the development connection string.
        /// </summary>
        private const string DevConnectionString =
            "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";

        /// <summary>
        /// Command line arguments.
        /// </summary>
        private readonly string[] commandLineArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionString" /> class.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the command line args is null.
        /// </exception>
        public ConnectionString(string[] commandLineArgs)
        {
            this.commandLineArgs = commandLineArgs ?? throw new ArgumentNullException(nameof(commandLineArgs));
        }

        /// <summary>
        /// Gets the connection string for Business One.
        /// </summary>
        /// <returns>
        /// The <see cref="string" /> to use to connect to Business One.
        /// </returns>
        /// <remarks>
        /// The command line arguments are checked to decide which connection string to use.
        /// </remarks>
        public string GetConnectionString()
        {
            if (IsBusinessOneConnectionString())
            {
                return BusinessOneConnectionString();
            }

            return DevConnectionString;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConnectionString" /> to <see cref="string" />.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static implicit operator string(ConnectionString connectionString)
        {
            return connectionString.GetConnectionString();
        }

        /// <summary>
        /// Returns the connection string from Business One on the command line args.
        /// </summary>
        /// <returns>
        /// The first argument on the command line arguments (according to Business One documentation).
        /// </returns>
        private string BusinessOneConnectionString()
        {
            return commandLineArgs[1];
        }

        /// <summary>
        /// Checks if the second argument is a match for a connection string.
        /// </summary>
        /// <returns>
        /// True if there is a match, false otherwise.
        /// </returns>
        private bool IsBusinessOneConnectionString()
        {
            // B1 always invokes the add-on with two command line arguments
            if (commandLineArgs.Length != 2)
            {
                return false;
            }

            // the first argument is the name of the executable, the second is the connection string
            string connectionString = commandLineArgs[1];

            return Regex.IsMatch(connectionString, @"[a-zA-Z0-9]{20,50}");
        }
    }
}
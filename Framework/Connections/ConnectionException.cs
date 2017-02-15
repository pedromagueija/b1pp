// <copyright filename="ConnectionException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error while attempting to connect to SAP Business One.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ConnectionException : Exception
    {
        private const int BusinessOneNotRunningErrorCode = -7202;
        private const int LoginInScreenLock = -1101;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        public ConnectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public ConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        /// <exception cref="SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult" /> is zero
        /// (0).
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        protected ConnectionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        internal static ConnectionException CreateFrom(COMException e)
        {
            string message = e.Message;

            if (e.ErrorCode == BusinessOneNotRunningErrorCode)
            {
                message = FormatConnectionMessage();
            }

            if (e.ErrorCode == LoginInScreenLock)
            {
                message = @"Login screen is in lock mode. Please login and run the addon again.";
            }

            return new ConnectionException(message, e);
        }

        private static string FormatConnectionMessage()
        {
            string runMode = IntPtr.Size == 4 ? @"32bit" : @"64bit";
            return $@"Unable to connect to SAP Business One. Is SAP Business One {runMode} running?";
        }
    }
}
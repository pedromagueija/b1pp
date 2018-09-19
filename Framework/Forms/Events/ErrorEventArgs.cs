// <copyright filename="ErrorEventArgs.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;

    /// <summary>
    /// Event arguments for the event handler error event .
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public Exception Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ErrorEventArgs(Exception exception)
        {
            Error = exception;
        }
    }
}
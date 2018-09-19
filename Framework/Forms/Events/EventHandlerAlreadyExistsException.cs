// <copyright filename="EventHandlerAlreadyExistsException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;

    /// <summary>
    /// Represents an error where a handler is already registered,
    /// <para />
    /// and an attempt is made to register it again.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class EventHandlerAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAlreadyExistsException" /> class.
        /// </summary>
        public EventHandlerAlreadyExistsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EventHandlerAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public EventHandlerAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="key">The key of the event handler.</param>
        public static EventHandlerAlreadyExistsException CreateFromKey(string key)
        {
            var exception = new EventHandlerAlreadyExistsException($"{key} is already registered.");
            return exception;
        }
    }
}
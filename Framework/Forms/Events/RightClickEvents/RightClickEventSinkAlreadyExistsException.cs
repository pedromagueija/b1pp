// <copyright filename="RightClickEventSinkAlreadyExistsException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.RightClickEvents
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when attempting to overwrite an existing right click event sink.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class RightClickEventSinkAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightClickEventSinkAlreadyExistsException" /> class.
        /// </summary>
        public RightClickEventSinkAlreadyExistsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightClickEventSinkAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RightClickEventSinkAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightClickEventSinkAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public RightClickEventSinkAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightClickEventSinkAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        /// information about the source or destination.
        /// </param>
        protected RightClickEventSinkAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
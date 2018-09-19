// <copyright filename="InvalidSpecificItemTypeException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error when converting a specific item to a requested type
    /// (e.g. converting a combo-box to a text edit).
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class InvalidSpecificItemTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSpecificItemTypeException" /> class.
        /// </summary>
        public InvalidSpecificItemTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSpecificItemTypeException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidSpecificItemTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSpecificItemTypeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public InvalidSpecificItemTypeException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSpecificItemTypeException" /> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        /// information about the source or destination.
        /// </param>
        protected InvalidSpecificItemTypeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
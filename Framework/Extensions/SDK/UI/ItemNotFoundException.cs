// <copyright filename="ItemNotFoundException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error when an item does not exist on the form's item collection.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        public ItemNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ItemNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ItemNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        /// information about the source or destination.
        /// </param>
        protected ItemNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
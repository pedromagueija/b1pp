// <copyright filename="SetPropertyException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error that occurred while attempting to set a value on a property.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class SetPropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetPropertyException"/> class.
        /// </summary>
        public SetPropertyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPropertyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SetPropertyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPropertyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public SetPropertyException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPropertyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
        protected SetPropertyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
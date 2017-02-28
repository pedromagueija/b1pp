// <copyright filename="B1EnumConvertException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error when attempting to convert an object to a SAP Business One enumeration.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class B1EnumConvertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B1EnumConvertException"/> class.
        /// </summary>
        public B1EnumConvertException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1EnumConvertException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public B1EnumConvertException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1EnumConvertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public B1EnumConvertException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1EnumConvertException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected B1EnumConvertException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
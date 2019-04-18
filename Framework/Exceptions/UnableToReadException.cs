// <copyright filename="UnableToReadException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace B1PP.Exceptions
{
    [Serializable]
    public class UnableToReadException : Exception
    {
        public UnableToReadException()
        {
        }

        public UnableToReadException(string message) : base(message)
        {
        }

        public UnableToReadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnableToReadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
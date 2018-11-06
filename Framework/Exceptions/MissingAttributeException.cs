// <copyright filename="MissingAttributeException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MissingAttributeException : Exception
    {
        public static MissingAttributeException Create(Type t, Type attributeType)
        {
            string message = $@"Type {t} does not contain a {attributeType} attribute.";
            return new MissingAttributeException(message);
        }
        
        public MissingAttributeException()
        {
        }

        public MissingAttributeException(string message) : base(message)
        {
        }

        public MissingAttributeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
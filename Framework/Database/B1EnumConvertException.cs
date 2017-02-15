// <copyright filename="B1EnumConvertException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class B1EnumConvertException : Exception
    {
        public B1EnumConvertException()
        {
        }

        public B1EnumConvertException(string message) : base(message)
        {
        }

        public B1EnumConvertException(string message, Exception inner) : base(message, inner)
        {
        }

        protected B1EnumConvertException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
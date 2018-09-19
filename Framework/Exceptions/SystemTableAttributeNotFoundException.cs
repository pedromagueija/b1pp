// <copyright filename="SystemTableAttributeNotFoundException.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SystemTableAttributeNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SystemTableAttributeNotFoundException()
        {
        }

        public SystemTableAttributeNotFoundException(string message) : base(message)
        {
        }

        public SystemTableAttributeNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SystemTableAttributeNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
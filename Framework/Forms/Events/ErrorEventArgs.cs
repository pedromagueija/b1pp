// <copyright filename="ErrorEventArgs.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;

    public class ErrorEventArgs : EventArgs
    {
        public Exception Error { get; }

        public ErrorEventArgs(Exception exception)
        {
            Error = exception;
        }
    }
}
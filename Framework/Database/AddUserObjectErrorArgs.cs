// <copyright filename="AddUserObjectErrorArgs.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;

    public class AddUserObjectErrorArgs : EventArgs
    {
        public string TableName { get; set; }
        public string ObjectName { get; set; }
        public string ErrorDescription { get; set; }
        public int ErrorCode { get; set; }
    }
}
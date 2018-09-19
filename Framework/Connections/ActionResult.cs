// <copyright filename="ActionResult.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Connections
{
    using System.Collections.Generic;
    using System.Linq;

    internal class ActionResult
    {
        private readonly IList<string> messages = new List<string>();

        public bool Success { get; set; }

        public IList<string> Messages
        {
            get
            {
                return new List<string>(messages);
            }
        }

        public string LastMessage
        {
            get
            {
                return Messages.Count > 0 ? Messages.Last() : string.Empty;
            }
        }

        public ActionResult()
        {
            Success = true;
        }

        public void Add(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                messages.Add(message);
            }
        }
    }
}
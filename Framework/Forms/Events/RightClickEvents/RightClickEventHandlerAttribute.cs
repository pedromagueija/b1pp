// <copyright filename="RightClickEventHandlerAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.RightClickEvents
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class RightClickEventHandlerAttribute : Attribute
    {
        public string ItemId { get; }
        public bool Before { get; }

        public RightClickEventHandlerAttribute() : this(string.Empty, false)
        {            
        }

        public RightClickEventHandlerAttribute(bool before) : this(string.Empty, before)
        {
        }

        public RightClickEventHandlerAttribute(string itemId) : this(itemId, false)
        {
        }

        public RightClickEventHandlerAttribute(string itemId, bool before)
        {
            ItemId = itemId;
            Before = before;
        }
    }
}
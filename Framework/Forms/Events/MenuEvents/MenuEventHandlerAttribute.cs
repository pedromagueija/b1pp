// <copyright filename="MenuEventHandlerAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.MenuEvents
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class MenuEventHandlerAttribute : Attribute
    {
        public bool Before { get; }
        public string MenuId { get; }

        public MenuEventHandlerAttribute(string menuId) : this(menuId, false)
        {
        }

        public MenuEventHandlerAttribute(string menuId, bool before)
        {
            Before = before;
            MenuId = menuId;
        }
    }
}
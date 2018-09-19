// <copyright filename="IRightClickEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.RightClickEvents
{
    using SAPbouiCOM;

    internal interface IRightClickEventListener
    {
        string Id { get; }

        bool OnRightClickEvent(ref ContextMenuInfo e, out bool bubbleEvent);
    }
}
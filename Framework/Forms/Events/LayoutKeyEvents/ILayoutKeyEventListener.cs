// <copyright filename="ILayoutKeyEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.LayoutKeyEvents
{
    using SAPbouiCOM;

    internal interface ILayoutKeyEventListener
    {
        void OnLayoutKeyEvent(ref LayoutKeyInfo e, out bool bubbleEvent);
    }
}
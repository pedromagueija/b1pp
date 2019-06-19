// <copyright filename="IMainMenuEventHandler.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.MenuEvents
{
    using SAPbouiCOM;

    internal interface IMainMenuEventHandler : IEventListener
    {
        void OnMenuEvent(ref MenuEvent e, out bool bubbleEvent);
    }
}
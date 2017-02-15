// <copyright filename="IRightClickEventSink.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.RightClickEvents
{
    using System.Collections.Generic;

    using SAPbouiCOM;

    /// <summary>
    /// Generic right click event sink that receives all right click events
    /// not handled by other specific handlers on the specified form types.
    /// </summary>
    public interface IRightClickEventSink
    {
        /// <summary>
        /// Form types for which this sink will be considered.
        /// </summary>
        /// <value>
        /// The form types.
        /// </value>
        IEnumerable<string> FormTypes { get; }

        /// <summary>
        /// Called when an unhandled right click event occurs.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <param name="bubbleEvent">The bubble event flag.</param>
        void OnRightClickEvent(ref ContextMenuInfo e, out bool bubbleEvent);
    }
}
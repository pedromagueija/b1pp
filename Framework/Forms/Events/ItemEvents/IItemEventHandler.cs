// <copyright filename="IItemEventHandler.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using SAPbouiCOM;

    /// <summary>
    /// Represents an item event listener that is notified
    /// when the item event matches the id of the identifier.
    /// </summary>
    internal interface IItemEventHandler
    {
        /// <summary>
        /// Gets the identifier of the listener.
        /// <para />
        /// This id should be a form id, form type or an event type.
        /// </summary>
        /// <value>
        /// The identifier of the listener.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Called when an item event is triggered.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <param name="bubbleEvent">The BubbleEvent flag.</param>
        void OnItemEvent(ref ItemEvent e, out bool bubbleEvent);
    }
}
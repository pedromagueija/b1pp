// <copyright filename="HandlerAddedEventArgs.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;

    using SAPbouiCOM;

    /// <summary>
    /// Information about the handler that was just added.
    /// </summary>
    public class HandlerAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public BoEventTypes EventType { get; }

        /// <summary>
        /// Gets the type of the form.
        /// </summary>
        /// <value>
        /// The type of the form.
        /// </value>
        public string FormType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerAddedEventArgs" /> class.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formType">Type of the form.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when formType is null.
        /// </exception>
        public HandlerAddedEventArgs(BoEventTypes eventType, string formType)
        {
            if (formType == null)
            {
                throw new ArgumentNullException(nameof(formType));
            }

            EventType = eventType;
            FormType = formType;
        }
    }
}
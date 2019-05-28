// <copyright filename="B1FormDataEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.FormDataEvents
{
    using System;
    using System.Collections.Generic;

    using SAPbouiCOM;

    internal class B1FormDataEventDispatcher
    {
        private Dictionary<string, IFormDataEventHandler> handlers =
            new Dictionary<string, IFormDataEventHandler>();

        private Application Application { get; set; }

        public void AddListener(FormDataEventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            
            handlers.Add(handler.Id, handler);
        }

        public event EventHandler<ErrorEventArgs> OnEventHandlerError = delegate { };

        public void RemoveListener(FormDataEventHandler handler)
        {
            if (handlers.ContainsKey(handler.Id))
            {
                handlers.Remove(handler.Id);
            }
        }

        /// <summary>
        /// Subscribes to the specified application item events.
        /// </summary>
        /// <param name="application">The application to subscribe to.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="application" /> is null.</exception>
        /// <exception cref="System.InvalidOperationException">Cannot subscribe twice to 'Application.ItemEvent'.</exception>
        public void Subscribe(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            
            if (Application != null)
            {
                throw new InvalidOperationException(@"Cannot subscribe twice to 'Application.ItemEvent'.");
            }

            Application = application;
            Application.FormDataEvent += OnFormDataEvent;
        }

        /// <summary>
        /// Stops listening to form data events.
        /// </summary>
        public void Unsubscribe()
        {
            if (Application == null)
            {
                return;
            }
            
            Application.FormDataEvent -= OnFormDataEvent;
            Application = null;
            handlers = new Dictionary<string, IFormDataEventHandler>();
        }

        /// <summary>
        /// Returns the handler id that can process the event. If no handler is able to handle the event, <langword>null</langword>
        /// is returned.
        /// </summary>
        /// <param name="e">
        /// Event information to use.
        /// </param>
        /// <returns>
        /// The handler id, or null when none is present.
        /// </returns>
        private string GetHandlerId(BusinessObjectInfo e)
        {
            // any event handler on a form is given priority, if none is present, 
            // any event handler of the form type is given priority, if none is present,
            // finally any event handler of the type of event is given priority.
            // When none is present null is returned.

            if (handlers.ContainsKey(e.FormUID))
            {
                return e.FormUID;
            }

            if (handlers.ContainsKey(e.FormTypeEx))
            {
                return e.FormTypeEx;
            }

            if (handlers.ContainsKey(e.EventType.ToString()))
            {
                return e.EventType.ToString();
            }

            return null;
        }

        private void OnFormDataEvent(ref BusinessObjectInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string handlerId = GetHandlerId(e);

                if (handlerId != null)
                {
                    handlers[handlerId].OnFormDataEvent(ref e, out bubbleEvent);
                }
            }
            catch (Exception exception)
            {
                OnEventHandlerError(this, new ErrorEventArgs(exception));
            }
        }
    }
}
// <copyright filename="B1ItemEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using System;

    using SAPbouiCOM;

    using Map = System.Collections.Generic.Dictionary<string, IItemEventHandler>;

    internal class B1ItemEventDispatcher
    {
        private readonly Map handlers = new Map();

        private Application Application { get; set; }

        public void AddListener(IItemEventHandler handler)
        {
            if (!handlers.ContainsKey(handler.Id))
            {
                handlers.Add(handler.Id, handler);
            }
        }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void RemoveListener(IItemEventHandler handler)
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
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="application" /> is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot subscribe twice to 'Application.ItemEvent'.
        /// </exception>
        public void Subscribe(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (Application != null)
            {
                throw new InvalidOperationException("Cannot subscribe twice to 'Application.ItemEvent'.");
            }

            Application = application;
            Application.ItemEvent += OnItemEvent;
        }

        /// <summary>
        /// Unsubscribing will remove all event listeners.
        /// </summary>
        public void Unsubscribe()
        {
            if (Application != null)
            {
                Application.ItemEvent -= OnItemEvent;
                Application = null;
                handlers.Clear();
            }
        }

        private bool Dispatch(string key, ItemEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (handlers.ContainsKey(key))
            {
                handlers[key].OnItemEvent(ref e, out bubbleEvent);
                return true;
            }

            return false;
        }

        private void OnItemEvent(string formId, ref ItemEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                if (Dispatch(e.FormUID, e, out bubbleEvent))
                {
                    return;
                }

                if (Dispatch(e.FormTypeEx, e, out bubbleEvent))
                {
                    return;
                }

                Dispatch(e.EventType.ToString(), e, out bubbleEvent);
            }
            catch (Exception exception)
            {
                EventHandlerError(this, new ErrorEventArgs(exception));
            }
        }
    }
}
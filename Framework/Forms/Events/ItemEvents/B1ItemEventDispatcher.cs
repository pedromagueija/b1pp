// <copyright filename="B1ItemEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.ItemEvents
{
    using System;

    using SAPbouiCOM;

    using ListenerCollection = System.Collections.Generic.Dictionary<string, IItemEventListener>;

    internal class B1ItemEventDispatcher
    {
        private readonly ListenerCollection Listeners = new ListenerCollection();

        private Application Application { get; set; }

        public void AddListener(IItemEventListener listener)
        {
            if (!Listeners.ContainsKey(listener.Id))
            {
                Listeners.Add(listener.Id, listener);
            }
        }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void RemoveListener(IItemEventListener listener)
        {
            if (Listeners.ContainsKey(listener.Id))
            {
                Listeners.Remove(listener.Id);
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
                Listeners.Clear();
            }
        }

        private bool Dispatch(string key, ItemEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (Listeners.ContainsKey(key))
            {
                Listeners[key].OnItemEvent(ref e, out bubbleEvent);
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
                EventHandlerError(null, new ErrorEventArgs(exception));
            }
        }
    }
}
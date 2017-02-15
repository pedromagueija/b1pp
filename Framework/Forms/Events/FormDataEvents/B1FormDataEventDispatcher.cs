// <copyright filename="B1FormDataEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.FormDataEvents
{
    using System;
    using System.Collections.Generic;

    using SAPbouiCOM;

    internal static class B1FormDataEventDispatcher
    {
        private static Dictionary<string, IFormDataEventListener> listeners =
            new Dictionary<string, IFormDataEventListener>();

        private static Application Application { get; set; }

        public static void AddListener(FormDataEventListener listener)
        {
            listeners.Add(listener.Id, listener);
        }

        public static event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public static void RemoveListener(FormDataEventListener listener)
        {
            if (listeners.ContainsKey(listener.Id))
            {
                listeners.Remove(listener.Id);
            }
        }

        /// <summary>
        /// Subscribes to the specified application item events.
        /// </summary>
        /// <param name="application">The application to subscribe to.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="application" /> is null.</exception>
        /// <exception cref="System.InvalidOperationException">Cannot subscribe twice to 'Application.ItemEvent'.</exception>
        public static void Subscribe(Application application)
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
            Application.FormDataEvent += OnFormDataEvent;
        }

        /// <summary>
        /// Unsubscribing will remove all event listeners.
        /// </summary>
        public static void Unsubscribe()
        {
            if (Application != null)
            {
                Application.FormDataEvent -= OnFormDataEvent;
                Application = null;
                listeners = new Dictionary<string, IFormDataEventListener>();
            }
        }

        private static void OnFormDataEvent(ref BusinessObjectInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string formUid = e.FormUID;
                if (listeners.ContainsKey(formUid))
                {
                    listeners[formUid].OnFormDataEvent(ref e, out bubbleEvent);
                    return;
                }

                string formType = e.FormTypeEx;
                if (listeners.ContainsKey(formType))
                {
                    listeners[formType].OnFormDataEvent(ref e, out bubbleEvent);
                    return;
                }

                string eventType = e.EventType.ToString();
                if (listeners.ContainsKey(eventType))
                {
                    listeners[eventType].OnFormDataEvent(ref e, out bubbleEvent);
                }
            }
            catch (Exception exception)
            {
                EventHandlerError(null, new ErrorEventArgs(exception));
            }
        }
    }
}
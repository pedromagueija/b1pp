// <copyright filename="B1LayoutKeyEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.LayoutKeyEvents
{
    using System;
    using System.Collections.Generic;

    using SAPbouiCOM;

    internal static class B1LayoutKeyEventDispatcher
    {
        private static readonly Dictionary<string, ILayoutKeyEventListener> eventListeners =
            new Dictionary<string, ILayoutKeyEventListener>();

        private static Application Application { get; set; }

        public static void AddListener(LayoutKeyEventListener listener)
        {
            eventListeners.Add(listener.Id, listener);
        }

        public static event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public static void RemoveListener(LayoutKeyEventListener listener)
        {
            eventListeners.Remove(listener.Id);
        }

        public static void Subscribe(Application application)
        {
            Application = application;
            Application.LayoutKeyEvent += OnLayoutKeyEvent;
        }

        public static void Unsubscribe()
        {
            if (Application != null)
            {
                Application.LayoutKeyEvent -= OnLayoutKeyEvent;
                Application = null;
                eventListeners.Clear();
            }
        }

        private static void OnLayoutKeyEvent(ref LayoutKeyInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string formUid = e.FormUID;
                if (eventListeners.ContainsKey(formUid))
                {
                    eventListeners[formUid].OnLayoutKeyEvent(ref e, out bubbleEvent);
                }
            }
            catch (Exception exception)
            {
                EventHandlerError(null, new ErrorEventArgs(exception));
            }
        }
    }
}
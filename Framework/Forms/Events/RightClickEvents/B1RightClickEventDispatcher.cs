// <copyright filename="B1RightClickEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.RightClickEvents
{
    using System;
    using System.Collections.Generic;

    using Extensions.SDK.UI;

    using JetBrains.Annotations;

    using SAPbouiCOM;

    internal static class B1RightClickEventDispatcher
    {
        /// <summary>
        /// Stores the event sink for right click events.
        /// </summary>
        [CanBeNull]
        private static IRightClickEventSink sink;

        private static List<IRightClickEventListener> rightClickEventListeners =
            new List<IRightClickEventListener>();

        private static Application Application { get; set; }

        public static void AddListener(IRightClickEventSink value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (sink != null)
                throw new RightClickEventSinkAlreadyExistsException();

            sink = value;

            foreach (string formType in sink.FormTypes)
            {
                B1EventFilterManager.Include(BoEventTypes.et_RIGHT_CLICK, formType);
            }            
        }

        public static void AddListener(IRightClickEventListener listener)
        {
            rightClickEventListeners.Add(listener);
        }

        public static event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public static void RemoveListener(IRightClickEventListener listener)
        {
            rightClickEventListeners.Remove(listener);
        }

        public static void Subscribe(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (Application != null)
            {
                throw new InvalidOperationException("Cannot subscribe twice to 'Application.RightClickEvent'.");
            }

            Application = application;
            Application.RightClickEvent += OnRightClickEvent;
        }

        internal static void Unsubscribe()
        {
            if (Application != null)
            {
                Application.RightClickEvent -= OnRightClickEvent;
                Application = null;
                rightClickEventListeners = new List<IRightClickEventListener>();
            }
        }

        private static void OnRightClickEvent(ref ContextMenuInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string activeFormId = Application.GetActiveFormId();
                if (string.IsNullOrEmpty(activeFormId))
                {
                    return;
                }

                IRightClickEventListener listener = rightClickEventListeners.Find(l => l.Id == activeFormId);
                if (listener != null)
                {
                    bool handled = listener.OnRightClickEvent(ref e, out bubbleEvent);
                    if (handled)
                        return;
                }

                // try to dispatch to sink
                sink?.OnRightClickEvent(ref e, out bubbleEvent);
            }
            catch (Exception exception)
            {
                EventHandlerError(null, new ErrorEventArgs(exception));
            }
        }
    }
}
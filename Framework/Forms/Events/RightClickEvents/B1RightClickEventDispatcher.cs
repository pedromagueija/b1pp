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

    internal class B1RightClickEventDispatcher
    {
        /// <summary>
        /// Stores the event sink for right click events.
        /// </summary>
        [CanBeNull]
        private IRightClickEventSink sink;

        private List<IRightClickEventListener> rightClickEventListeners =
            new List<IRightClickEventListener>();

        private Application Application { get; set; }

        public void AddListener(IRightClickEventSink value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (sink != null)
                throw new RightClickEventSinkAlreadyExistsException();

            sink = value;

            foreach (var formType in sink.FormTypes)
            {
                B1EventFilterManager.Include(BoEventTypes.et_RIGHT_CLICK, formType);
            }            
        }

        public void AddListener(IRightClickEventListener listener)
        {
            rightClickEventListeners.Add(listener);
        }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void RemoveListener(IRightClickEventListener listener)
        {
            rightClickEventListeners.Remove(listener);
        }

        public void Subscribe(Application application)
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

        internal void Unsubscribe()
        {
            if (Application != null)
            {
                Application.RightClickEvent -= OnRightClickEvent;
                Application = null;
                rightClickEventListeners = new List<IRightClickEventListener>();
            }
        }

        private void OnRightClickEvent(ref ContextMenuInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string activeFormId = Application.GetActiveFormId();
                if (string.IsNullOrEmpty(activeFormId))
                {
                    return;
                }

                var listener = rightClickEventListeners.Find(l => l.Id == activeFormId);
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
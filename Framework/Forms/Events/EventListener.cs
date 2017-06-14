// <copyright filename="EventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System.Collections.Generic;

    using ItemEvents;

    using LayoutKeyEvents;

    using MenuEvents;

    using RightClickEvents;

    /// <summary>
    /// Scans the given object for event handlers and subscribes them with the appropriate listeners.
    /// </summary>
    public class EventListener
    {
        private readonly IList<IEventListener> listeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListener"/> class.
        /// </summary>
        /// <param name="form">The form.</param>
        public EventListener(B1Session session, IFormInstance form):this(session, form, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListener"/> class.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="subordinates">Other objects that contain.</param>
        public EventListener(B1Session session, IFormInstance form, params object[] subordinates)
        {
            var delegates = subordinates ?? new object[0];

            listeners = new List<IEventListener>()
            {
                session.CreateFormMenuEventListener(form),
                session.CreateRightClickEventListener(form),
                session.CreateFormItemEventListener(form, delegates),
                session.CreateFormDataEventListener(form),
                session.CreateLayoutKeyEventListener(form)
            };
        }

        /// <summary>
        /// Subscribes this instance.
        /// </summary>
        public void Subscribe()
        {
            foreach (IEventListener listener in listeners)
            {
                listener.HandlerAdded += OnHandlerAdded;
                listener.Subscribe();
            }
        }

        /// <summary>
        /// Unsubscribes this instance.
        /// </summary>
        public void Unsubscribe()
        {
            foreach (IEventListener listener in listeners)
            {
                listener.HandlerAdded -= OnHandlerAdded;
                listener.Unsubscribe();
            }
        }

        private void OnHandlerAdded(object sender, HandlerAddedEventArgs e)
        {
            B1EventFilterManager.Include(e.EventType, e.FormType);
        }
    }
}
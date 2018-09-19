// <copyright filename="FormItemEventHandler.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using System;

    using SAPbouiCOM;

    internal class FormItemEventHandler : IItemEventHandler, IFilterable, IEventListener
    {
        private readonly B1ItemEventDispatcher dispatcher;
        private readonly IFormInstance form;
        private readonly HandlersCollection<ItemEvent> handlers = new HandlersCollection<ItemEvent>();
        private readonly object[] subordinates;


        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public FormItemEventHandler(B1ItemEventDispatcher dispatcher, IFormInstance form) : this(dispatcher, form, null)
        {
        }

        public FormItemEventHandler(B1ItemEventDispatcher dispatcher, IFormInstance form, params object[] subordinates)
        {
            this.dispatcher = dispatcher;
            this.form = form;
            this.subordinates = subordinates ?? new object[0];
        }

        /// <summary>
        /// Subscribes the objects own handlers and the subordinate object handlers.
        /// </summary>
        public void Subscribe()
        {
            AddEventHandlers(form, form.FormType);
            foreach (var subordinate in subordinates)
            {
                AddEventHandlers(subordinate, form.FormType);
            }

            dispatcher.AddListener(this);
        }

        public void Unsubscribe()
        {
            dispatcher.RemoveListener(this);
        }

        public void OnItemEvent(ref ItemEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (e.BeforeAction)
            {
                handlers.HandleBefore(e, out bubbleEvent);
            }
            else
            {
                handlers.HandleAfter(e);
            }
        }

        public event EventHandler<HandlerAddedEventArgs> HandlerAdded = delegate { };

        private void AddEventHandlers(object userForm, string formType)
        {
            var handlerMethods = ClassHelper.FindAnnotatedMethods<ItemEventHandlerAttribute>(userForm);

            foreach (var method in handlerMethods)
            {
                var attribute = method.GetAttribute<ItemEventHandlerAttribute>();
                var signature = ItemEventSignature.Create(method);

                if (attribute.BeforeAction)
                {
                    var action = BeforeAction<ItemEvent>.CreateNew(signature, method, userForm);
                    handlers.AddBefore(action);
                }
                else
                {
                    var action = AfterAction<ItemEvent>.CreateNew(signature, method, userForm);
                    handlers.AddAfter(action);
                }

                HandlerAdded(this, new HandlerAddedEventArgs(attribute.EventType, formType));
            }
        }
    }
}
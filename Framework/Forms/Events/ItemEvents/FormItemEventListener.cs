// <copyright filename="FormItemEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.ItemEvents
{
    using System;
    using System.Reflection;

    using SAPbouiCOM;

    internal class FormItemEventListener : IItemEventListener, IFilterable, IEventListener
    {
        private readonly HandlersCollection<ItemEvent> handlers = new HandlersCollection<ItemEvent>();

        private readonly B1ItemEventDispatcher dispatcher;
        private readonly IFormInstance form;
        private readonly object[] subordinates;


        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public FormItemEventListener(B1ItemEventDispatcher dispatcher, IFormInstance form) : this(dispatcher, form, null)
        {
        }

        public FormItemEventListener(B1ItemEventDispatcher dispatcher, IFormInstance form, params object[] subordinates)
        {
            this.dispatcher = dispatcher;
            this.form = form;
            this.subordinates = subordinates ?? new object[0];
        }


        public void OnItemEvent(ref ItemEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if(e.BeforeAction)
                handlers.HandleBefore(e, out bubbleEvent);
            else
                handlers.HandleAfter(e);
        }

        public event EventHandler<HandlerAddedEventArgs> HandlerAdded = delegate { };

        /// <summary>
        /// Subscribes the objects own handlers and the subordinate object handlers.
        /// </summary>
        public void Subscribe()
        {
            AddEventHandlers(form, form.FormType);
            foreach (object subordinate in subordinates)
            {
                AddEventHandlers(subordinate, form.FormType);
            }

            dispatcher.AddListener(this);
        }

        public void Unsubscribe()
        {
            dispatcher.RemoveListener(this);
        }

        private void AddEventHandlers(object userForm, string formType)
        {
            var handlerMethods = ClassHelper.FindAnnotatedMethods<ItemEventHandlerAttribute>(userForm);

            foreach (MethodInfo method in handlerMethods)
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
// <copyright filename="FormDataEventHandler.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.FormDataEvents
{
    using System;
    using System.Collections.Generic;

    using SAPbouiCOM;

    internal class FormDataEventHandler : IFormDataEventHandler, IEventListener
    {
        private readonly Dictionary<FormDataEventHandlerAttribute, Action<BusinessObjectInfo>> after =
            new Dictionary<FormDataEventHandlerAttribute, Action<BusinessObjectInfo>>();

        private readonly Dictionary<FormDataEventHandlerAttribute, Func<BusinessObjectInfo, bool>> before =
            new Dictionary<FormDataEventHandlerAttribute, Func<BusinessObjectInfo, bool>>();

        private readonly B1FormDataEventDispatcher dispatcher;

        private readonly IFormInstance form;

        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public FormDataEventHandler(IFormInstance form, B1FormDataEventDispatcher dispatcher)
        {
            this.form = form;
            this.dispatcher = dispatcher;
        }

        public void Subscribe()
        {
            AddEventHandlers(form);

            dispatcher.AddListener(this);
        }

        public void Unsubscribe()
        {
            dispatcher.RemoveListener(this);
        }

        public void OnFormDataEvent(ref BusinessObjectInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            var key = new FormDataEventHandlerAttribute(e.EventType, e.BeforeAction);

            if (e.BeforeAction && before.ContainsKey(key))
            {
                bubbleEvent = before[key].Invoke(e);
                return;
            }

            if (!e.BeforeAction && after.ContainsKey(key))
            {
                after[key].Invoke(e);
            }
        }

        public event EventHandler<HandlerAddedEventArgs> HandlerAdded = delegate { };

        private void AddEventHandlers(IFormInstance userForm)
        {
            var handlerMethods = ClassHelper.FindAnnotatedMethods<FormDataEventHandlerAttribute>(userForm);

            foreach (var method in handlerMethods)
            {
                var attribute = method.GetAttribute<FormDataEventHandlerAttribute>();

                if (attribute.Before)
                {
                    before.Add(attribute, method.CreateBeforeEventDelegate<BusinessObjectInfo>(userForm));
                }
                else
                {
                    after.Add(attribute, method.CreateAfterEventDelegate<BusinessObjectInfo>(userForm));
                }

                HandlerAdded(this, new HandlerAddedEventArgs(attribute.EventType, userForm.FormType));
            }
        }
    }
}
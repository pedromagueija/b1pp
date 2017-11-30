// <copyright filename="FormDataEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.FormDataEvents
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using SAPbouiCOM;

    internal class FormDataEventListener : IFormDataEventListener, IEventListener
    {
        private readonly Dictionary<FormDataEventHandlerAttribute, Action<BusinessObjectInfo>> after =
            new Dictionary<FormDataEventHandlerAttribute, Action<BusinessObjectInfo>>();

        private readonly Dictionary<FormDataEventHandlerAttribute, Func<BusinessObjectInfo, bool>> before =
            new Dictionary<FormDataEventHandlerAttribute, Func<BusinessObjectInfo, bool>>();

        private readonly IFormInstance form;
        private readonly B1FormDataEventDispatcher dispatcher;

        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public FormDataEventListener(IFormInstance form, B1FormDataEventDispatcher dispatcher)
        {
            this.form = form;
            this.dispatcher = dispatcher;
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

        public void Subscribe()
        {
            AddEventHandlers(form);

            dispatcher.AddListener(this);
        }

        public void Unsubscribe()
        {
            dispatcher.RemoveListener(this);
        }

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
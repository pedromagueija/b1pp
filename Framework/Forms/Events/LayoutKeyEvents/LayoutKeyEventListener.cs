// <copyright filename="LayoutKeyEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.LayoutKeyEvents
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using SAPbouiCOM;

    internal class LayoutKeyEventListener : ILayoutKeyEventListener, IEventListener
    {
        private readonly Dictionary<LayoutKeyEventHandlerAttribute, Action<LayoutKeyInfo>> after =
            new Dictionary<LayoutKeyEventHandlerAttribute, Action<LayoutKeyInfo>>();

        private readonly Dictionary<LayoutKeyEventHandlerAttribute, Func<LayoutKeyInfo, bool>> before =
            new Dictionary<LayoutKeyEventHandlerAttribute, Func<LayoutKeyInfo, bool>>();

        private readonly B1LayoutKeyEventDispatcher dispatcher;
        private readonly IFormInstance form;


        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public LayoutKeyEventListener(B1LayoutKeyEventDispatcher dispatcher, IFormInstance form)
        {
            this.dispatcher = dispatcher;
            this.form = form;
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


        public void OnLayoutKeyEvent(ref LayoutKeyInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            var key = new LayoutKeyEventHandlerAttribute(e.BeforeAction);

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
            var handlerMethods = ClassHelper.FindAnnotatedMethods<LayoutKeyEventHandlerAttribute>(userForm);

            foreach (MethodInfo method in handlerMethods)
            {
                var attribute = method.GetAttribute<LayoutKeyEventHandlerAttribute>();
                if (attribute.Before)
                {
                    before.Add(attribute, method.CreateBeforeEventDelegate<LayoutKeyInfo>(userForm));
                }
                else
                {
                    after.Add(attribute, method.CreateAfterEventDelegate<LayoutKeyInfo>(userForm));
                }

                HandlerAdded(this, new HandlerAddedEventArgs(BoEventTypes.et_PRINT_LAYOUT_KEY, userForm.FormType));
            }
        }
    }
}
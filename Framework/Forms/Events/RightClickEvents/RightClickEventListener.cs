// <copyright filename="RightClickEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.RightClickEvents
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using SAPbouiCOM;

    internal class RightClickEventListener : IRightClickEventListener, IEventListener
    {
        private readonly Dictionary<string, Action<ContextMenuInfo>> after =
            new Dictionary<string, Action<ContextMenuInfo>>();

        private readonly Dictionary<string, Func<ContextMenuInfo, bool>> before =
            new Dictionary<string, Func<ContextMenuInfo, bool>>();

        private readonly B1RightClickEventDispatcher dispatcher;
        private readonly IFormInstance form;

        public string Id
        {
            get
            {
                return form.FormId;
            }
        }

        public RightClickEventListener(B1RightClickEventDispatcher dispatcher, IFormInstance form)
        {
            this.dispatcher = dispatcher;
            this.form = form;
        }

        public bool OnRightClickEvent(ref ContextMenuInfo e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (e.BeforeAction && before.ContainsKey(e.ItemUID))
            {
                bubbleEvent = before[e.ItemUID].Invoke(e);
                return true;
            }

            if (!e.BeforeAction && after.ContainsKey(e.ItemUID))
            {
                after[e.ItemUID].Invoke(e);
                return true;
            }

            return false;
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
            var handlerMethods = ClassHelper.FindAnnotatedMethods<RightClickEventHandlerAttribute>(userForm);

            foreach (MethodInfo method in handlerMethods)
            {
                var attribute = method.GetAttribute<RightClickEventHandlerAttribute>();
                if (attribute.Before)
                {
                    before.Add(attribute.ItemId, method.CreateBeforeEventDelegate<ContextMenuInfo>(userForm));
                }
                else
                {
                    after.Add(attribute.ItemId, method.CreateAfterEventDelegate<ContextMenuInfo>(userForm));
                }

                HandlerAdded(this, new HandlerAddedEventArgs(BoEventTypes.et_RIGHT_CLICK, userForm.FormType));
            }
        }
    }
}
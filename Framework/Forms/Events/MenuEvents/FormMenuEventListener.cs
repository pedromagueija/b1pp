// <copyright filename="FormMenuEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.MenuEvents
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using SAPbouiCOM;

    internal class FormMenuEventListener : IFormMenuEventListener, IEventListener
    {
        private readonly Dictionary<string, Action<MenuEvent>> after = new Dictionary<string, Action<MenuEvent>>();

        private readonly Dictionary<string, Func<MenuEvent, bool>> before =
            new Dictionary<string, Func<MenuEvent, bool>>();

        private readonly IFormInstance form;

        public string FormId
        {
            get
            {
                return form.FormId;
            }
        }

        public FormMenuEventListener(IFormInstance form)
        {
            this.form = form;
        }

        public bool OnMenuEvent(ref MenuEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (e.BeforeAction && before.ContainsKey(e.MenuUID))
            {
                bubbleEvent = before[e.MenuUID].Invoke(e);
                return true;
            }

            if (!e.BeforeAction && after.ContainsKey(e.MenuUID))
            {
                after[e.MenuUID].Invoke(e);
                return true;
            }

            return false;
        }

        public event EventHandler<HandlerAddedEventArgs> HandlerAdded = delegate { };

        public void Subscribe()
        {
            AddEventHandlers(form);

            B1MenuEventDispatcher.AddListener(this);
        }

        public void Unsubscribe()
        {
            B1MenuEventDispatcher.RemoveListener(this);
        }

        private void AddEventHandlers(IFormInstance userForm)
        {
            var handlerMethods = ClassHelper.FindAnnotatedMethods<MenuEventHandlerAttribute>(userForm);

            foreach (MethodInfo method in handlerMethods)
            {
                var attribute = method.GetAttribute<MenuEventHandlerAttribute>();
                try
                {
                    if (attribute.Before)
                    {
                        before.Add(attribute.MenuId, method.CreateBeforeEventDelegate<MenuEvent>(userForm));
                    }
                    else
                    {
                        after.Add(attribute.MenuId, method.CreateAfterEventDelegate<MenuEvent>(userForm));
                    }
                }
                catch (ArgumentException e)
                {
                    var exception =
                        new EventHandlerAlreadyExistsException(
                            $"A menu event handler for {attribute.MenuId} already exists. Duplicate method {method.Name}.",
                            e);
                    throw exception;
                }


                HandlerAdded(this, new HandlerAddedEventArgs(BoEventTypes.et_MENU_CLICK, userForm.FormType));
            }
        }
    }
}
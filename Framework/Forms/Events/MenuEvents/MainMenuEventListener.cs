// <copyright filename="MainMenuEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.MenuEvents
{
    using System;
    using System.Reflection;

    using SAPbouiCOM;

    using AfterEventMap = System.Collections.Generic.Dictionary<string, System.Action<SAPbouiCOM.MenuEvent>>;
    using BeforeEventMap = System.Collections.Generic.Dictionary<string, System.Func<SAPbouiCOM.MenuEvent, bool>>;

    internal class MainMenuEventListener : IMainMenuEventListener, IEventListener
    {
        private readonly IMainMenuInstance main;

        private readonly AfterEventMap after = new AfterEventMap();

        private readonly BeforeEventMap before = new BeforeEventMap();

        public MainMenuEventListener(IMainMenuInstance main)
        {
            this.main = main;
        }

        public void OnMenuEvent(ref MenuEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (e.BeforeAction && before.ContainsKey(e.MenuUID))
            {
                bubbleEvent = before[e.MenuUID].Invoke(e);
                return;
            }

            if (!e.BeforeAction && after.ContainsKey(e.MenuUID))
            {
                after[e.MenuUID].Invoke(e);
            }
        }

        public event EventHandler<HandlerAddedEventArgs> HandlerAdded = delegate { };

        public void Subscribe()
        {
            AddEventHandlers(main);

            B1MenuEventDispatcher.AddMainMenuListener(this);
        }

        private void AddEventHandlers(IMainMenuInstance mainMenu)
        {
            var handlerMethods = ClassHelper.FindAnnotatedMethods<MenuEventHandlerAttribute>(mainMenu);

            foreach (MethodInfo method in handlerMethods)
            {
                var attribute = method.GetAttribute<MenuEventHandlerAttribute>();
                try
                {
                    if (attribute.Before)
                    {
                        before.Add(attribute.MenuId, method.CreateBeforeEventDelegate<MenuEvent>(mainMenu));
                    }
                    else
                    {
                        after.Add(attribute.MenuId, method.CreateAfterEventDelegate<MenuEvent>(mainMenu));
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


                HandlerAdded(this, new HandlerAddedEventArgs(BoEventTypes.et_MENU_CLICK, @"ALL_FORMS"));
            }
        }

        public void Unsubscribe()
        {
            B1MenuEventDispatcher.RemoveMainMenuListener();
        }
    }
}
// <copyright filename="B1EventsManager.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ApplicationEvents;

    using Extensions.Common;

    using FormDataEvents;

    using ItemEvents;

    using LayoutKeyEvents;

    using MenuEvents;

    using RightClickEvents;

    using SAPbouiCOM;

    /// <summary>
    /// Manages the events session.
    /// </summary>
    public class B1EventsManager
    {
        private static B1EventsManager instance;

        private Application application;
        private Assembly assembly;
        private Type[] assemblyTypes;


        private IMainMenuInstance mainMenu;

        /// <summary>
        /// Gets the instance of the events manager.
        /// </summary>
        public static B1EventsManager Instance
        {
            get
            {
                return instance ?? (instance = new B1EventsManager());
            }
        }

        private ApplicationEventDispatcher ApplicationEventDispatcher { get; set; }

        private B1FormDataEventDispatcher B1FormDataEventDispatcher { get; set; }

        private B1MenuEventDispatcher B1MenuEventDispatcher { get; set; }

        private B1ItemEventDispatcher B1ItemEventDispatcher { get; set; }

        private B1RightClickEventDispatcher B1RightClickEventDispatcher { get; set; }

        private B1LayoutKeyEventDispatcher B1LayoutKeyEventDispatcher { get; set; }

        private static Func<Type, bool> IsSystemForm
        {
            get
            {
                return t => t.GetCustomAttributes(typeof(B1SystemFormTypeAttribute), true).Any();
            }
        }

        private Func<Type, bool> IsRightClickEventSink
        {
            get
            {
                return t => t.Implements(typeof(IRightClickEventSink));
            }
        }

        /// <summary>
        /// Starts up the events managing system.
        /// </summary>
        public void Initialize(Application app, IApplicationEventsHandler handler,
            IMainMenuInstance mainMenuInstance = null)
        {
            application = app;
            assembly = GetAssembly(handler);
            assemblyTypes = GetAssemblyTypes(assembly);
            mainMenu = GetMainMenu(mainMenuInstance);

            B1EventFilterManager.Initialize(app);

            CreateDispatchers(handler);
            StartEventDispatchers();

            AddSystemFormLoadListeners();
            AddEventSinks();

            SetMainMenu();
        }

        /// <summary>
        /// Triggered when an event handler throws an exception
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnError = delegate { };

        /// <summary>
        /// Terminates the event managing system.
        /// </summary>
        public void Terminate()
        {
            B1ItemEventDispatcher.Unsubscribe();
            B1MenuEventDispatcher.Unsubscribe();
            B1RightClickEventDispatcher.Unsubscribe();
            B1LayoutKeyEventDispatcher.Unsubscribe();
            B1FormDataEventDispatcher.Unsubscribe();
            ApplicationEventDispatcher.Unsubscribe();
        }

        internal IEventListener CreateFormDataEventListener(IFormInstance form)
        {
            return new FormDataEventHandler(form, B1FormDataEventDispatcher);
        }

        internal IEventListener CreateFormItemEventListener(IFormInstance form, params object[] subordinates)
        {
            return new FormItemEventHandler(B1ItemEventDispatcher, form, subordinates);
        }

        internal IEventListener CreateFormMenuEventListener(IFormInstance form)
        {
            return new FormMenuEventListener(form, B1MenuEventDispatcher);
        }

        internal IEventListener CreateLayoutKeyEventListener(IFormInstance form)
        {
            return new LayoutKeyEventListener(B1LayoutKeyEventDispatcher, form);
        }

        internal IEventListener CreateRightClickEventListener(IFormInstance form)
        {
            return new RightClickEventListener(B1RightClickEventDispatcher, form);
        }

        private void AddEventSink(Type type)
        {
            var sink = type.CreateInstance<IRightClickEventSink>();
            B1RightClickEventDispatcher.AddListener(sink);
        }

        private void AddEventSinks()
        {
            var eventSinks = FindTypes(IsRightClickEventSink);

            foreach (var type in eventSinks)
            {
                AddEventSink(type);
            }
        }

        private void AddSystemFormLoadListener(Type classType)
        {
            var attribute = GetAttribute<B1SystemFormTypeAttribute>(classType);
            var formType = attribute.FormType;

            var loadListener = new SystemFormLoadHandler(application, formType, classType);

            B1ItemEventDispatcher.AddListener(loadListener);
            B1EventFilterManager.Include(BoEventTypes.et_FORM_LOAD, formType);
        }

        private void AddSystemFormLoadListeners()
        {
            var systemForms = FindTypes(IsSystemForm);

            foreach (var systemForm in systemForms)
            {
                AddSystemFormLoadListener(systemForm);
            }
        }

        private void CreateDispatchers(IApplicationEventsHandler handler)
        {
            ApplicationEventDispatcher = new ApplicationEventDispatcher(handler);
            B1FormDataEventDispatcher = new B1FormDataEventDispatcher();
            B1MenuEventDispatcher = new B1MenuEventDispatcher();
            B1ItemEventDispatcher = new B1ItemEventDispatcher();
            B1RightClickEventDispatcher = new B1RightClickEventDispatcher();
            B1LayoutKeyEventDispatcher = new B1LayoutKeyEventDispatcher();
        }

        private IEventListener CreateMainMenuEventListener(IMainMenuInstance mainMenuInstance)
        {
            return new MainMenuEventListener(mainMenuInstance, B1MenuEventDispatcher);
        }

        private IEnumerable<Type> FindTypes(Func<Type, bool> predicate)
        {
            if (assemblyTypes == null)
            {
                assemblyTypes = GetAssemblyTypes(assembly);
            }

            var types = assemblyTypes.Where(predicate);
            return types;
        }

        private Assembly GetAssembly(IApplicationEventsHandler handler)
        {
            return handler.GetType().Assembly;
        }

        private Type[] GetAssemblyTypes(Assembly a)
        {
            return a.GetTypes();
        }

        private T GetAttribute<T>(Type systemForm)
        {
            var attribute = systemForm.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            return (T) attribute;
        }

        private static IMainMenuInstance GetMainMenu(IMainMenuInstance mainMenuInstance)
        {
            return mainMenuInstance ?? new NullMainMenuInstance();
        }

        private void OnEventHandlerError(object sender, ErrorEventArgs e)
        {
            // todo: prevent the handler also throwing errors
            OnError(sender, e);
        }

        private void SetMainMenu()
        {
            var listener = CreateMainMenuEventListener(mainMenu);
            listener.Subscribe();
            B1EventFilterManager.Include(BoEventTypes.et_MENU_CLICK, @"ALL_FORMS");
        }

        private void StartEventDispatchers()
        {
            ApplicationEventDispatcher.Subscribe(application);
            B1ItemEventDispatcher.Subscribe(application);
            B1MenuEventDispatcher.Subscribe(application);
            B1RightClickEventDispatcher.Subscribe(application);
            B1FormDataEventDispatcher.Subscribe(application);
            B1LayoutKeyEventDispatcher.Subscribe(application);

            ApplicationEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1MenuEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1RightClickEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1ItemEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1FormDataEventDispatcher.OnEventHandlerError += OnEventHandlerError;
            B1LayoutKeyEventDispatcher.EventHandlerError += OnEventHandlerError;
        }
    }
}
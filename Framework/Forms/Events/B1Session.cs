// <copyright filename="B1Session.cs" project="Framework">
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

    using JetBrains.Annotations;

    using LayoutKeyEvents;

    using MenuEvents;

    using RightClickEvents;

    using SAPbouiCOM;

    /// <summary>
    /// Manages the events session.
    /// </summary>
    public class B1Session
    {
        private readonly Application application;

        private readonly IApplicationEventsHandler applicationEventsHandler;
        private readonly Assembly assembly;
        private readonly IMainMenuInstance mainMenu;

        private B1ApplicationEventDispatcher B1ApplicationEventDispatcher { get; }
        private B1FormDataEventDispatcher B1FormDataEventDispatcher { get; }

        private B1MenuEventDispatcher B1MenuEventDispatcher { get; }

        private B1ItemEventDispatcher B1ItemEventDispatcher { get; }

        private B1RightClickEventDispatcher B1RightClickEventDispatcher { get; }

        private B1LayoutKeyEventDispatcher B1LayoutKeyEventDispatcher { get; }

        private static Func<Type, bool> IsSystemForm
        {
            get
            {
                return t => t.GetCustomAttributes(typeof(B1SystemFormTypeAttribute), true).Any();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1Session" /> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="applicationEventsHandler">
        /// The instance responsible for application event handling.
        /// </param>
        /// <param name="mainMenu">
        /// The main menu instance.
        /// </param>
        public B1Session(
            Application application,
            IApplicationEventsHandler applicationEventsHandler,
            [CanBeNull] IMainMenuInstance mainMenu)
        {
            this.application = application;
            assembly = applicationEventsHandler.GetType().Assembly;
            this.applicationEventsHandler = applicationEventsHandler;
            this.mainMenu = mainMenu ?? new NullMainMenuInstance();

            B1ApplicationEventDispatcher = new B1ApplicationEventDispatcher();
            B1FormDataEventDispatcher = new B1FormDataEventDispatcher();
            B1MenuEventDispatcher = new B1MenuEventDispatcher();
            B1ItemEventDispatcher = new B1ItemEventDispatcher();
            B1RightClickEventDispatcher = new B1RightClickEventDispatcher();
            B1LayoutKeyEventDispatcher = new B1LayoutKeyEventDispatcher();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1Session" /> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="applicationEventsHandler">
        /// The instance responsible for application event handling.
        /// </param>
        public B1Session(
            Application application,
            IApplicationEventsHandler applicationEventsHandler) : this(application, applicationEventsHandler, null)
        {
        }

        /// <summary>
        /// Starts up the events managing system.
        /// </summary>
        public void Initialize()
        {
            B1EventFilterManager.Initialize(application);

            SetApplicationEventListener();
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
            B1ApplicationEventDispatcher.Unsubscribe();
        }

        internal IEventListener CreateFormDataEventListener(IFormInstance form)
        {
            return new FormDataEventListener(form, B1FormDataEventDispatcher);
        }

        internal IEventListener CreateFormMenuEventListener(IFormInstance form)
        {
            return new FormMenuEventListener(form, B1MenuEventDispatcher);
        }

        private void AddEventSink(Type type)
        {
            var sink = type.CreateInstance<IRightClickEventSink>();
            B1RightClickEventDispatcher.AddListener(sink);
        }

        private void AddEventSinks()
        {
            var eventSinks = FindEventSinks();

            foreach (var type in eventSinks)
            {
                AddEventSink(type);
            }
        }

        private void AddSystemFormLoadListener(Type classType)
        {
            var attribute = GetAttribute<B1SystemFormTypeAttribute>(classType);
            var formType = attribute.FormType;

            var loadListener = new SystemFormLoadListener(application, formType, classType);

            B1ItemEventDispatcher.AddListener(loadListener);
            B1EventFilterManager.Include(BoEventTypes.et_FORM_LOAD, formType);
        }

        private void AddSystemFormLoadListeners()
        {
            var systemForms = FindSystemFormsInAssembly();

            foreach (var systemForm in systemForms)
            {
                AddSystemFormLoadListener(systemForm);
            }
        }

        private IEnumerable<Type> FindEventSinks()
        {
            var systemForms = assembly.GetTypes()
                .Where(IsIRightClickEventSinkType());

            return systemForms;
        }

        private IEnumerable<Type> FindSystemFormsInAssembly()
        {
            var systemForms = assembly.GetTypes()
                .Where(IsSystemForm);

            return systemForms;
        }

        private T GetAttribute<T>(Type systemForm)
        {
            var attribute = systemForm.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            return (T) attribute;
        }

        private Func<Type, bool> IsIRightClickEventSinkType()
        {
            return t => t.Implements(typeof(IRightClickEventSink));
        }

        private void OnEventHandlerError(object sender, ErrorEventArgs e)
        {
            // todo: prevent the handler also throwing errors
            OnError(sender, e);
        }

        private void SetApplicationEventListener()
        {
            B1ApplicationEventDispatcher.SetListener(applicationEventsHandler);
        }

        private void SetMainMenu()
        {
            var listener = CreateMainMenuEventListener(mainMenu);
            listener.Subscribe();
            B1EventFilterManager.Include(BoEventTypes.et_MENU_CLICK, @"ALL_FORMS");
        }

        private IEventListener CreateMainMenuEventListener(IMainMenuInstance instance)
        {
            return new MainMenuEventListener(instance, B1MenuEventDispatcher);
        }

        private void StartEventDispatchers()
        {
            B1ApplicationEventDispatcher.Subscribe(application);
            B1ItemEventDispatcher.Subscribe(application);
            B1MenuEventDispatcher.Subscribe(application);
            B1RightClickEventDispatcher.Subscribe(application);
            B1FormDataEventDispatcher.Subscribe(application);
            B1LayoutKeyEventDispatcher.Subscribe(application);

            B1ApplicationEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1MenuEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1RightClickEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1ItemEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1FormDataEventDispatcher.EventHandlerError += OnEventHandlerError;
            B1LayoutKeyEventDispatcher.EventHandlerError += OnEventHandlerError;
        }

        internal IEventListener CreateFormItemEventListener(IFormInstance form, params object[] subordinates)
        {
            return new FormItemEventListener(B1ItemEventDispatcher, form, subordinates);
        }

        internal IEventListener CreateRightClickEventListener(IFormInstance form)
        {
            return new RightClickEventListener(B1RightClickEventDispatcher, form);
        }

        internal IEventListener CreateLayoutKeyEventListener(IFormInstance form)
        {
            return new LayoutKeyEventListener(B1LayoutKeyEventDispatcher, form);
        }
    }
}
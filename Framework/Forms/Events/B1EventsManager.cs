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

    using JetBrains.Annotations;

    using LayoutKeyEvents;

    using MenuEvents;

    using RightClickEvents;

    using SAPbouiCOM;

    /// <summary>
    /// Manages events from SAP Business One.
    /// </summary>
    public class B1EventsManager
    {
        private readonly Application application;

        private readonly IApplicationInstance applicationInstance;
        private readonly IMainMenuInstance mainMenu;
        private readonly Assembly assembly;

        private B1ApplicationEventDispatcher B1ApplicationEventDispatcher { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1EventsManager" /> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="applicationInstance">
        /// The instance responsible for application event handling.
        /// </param>
        /// <param name="mainMenu">
        /// The main menu instance.
        /// </param>
        public B1EventsManager(
            Application application,
            IApplicationInstance applicationInstance,
            [CanBeNull] IMainMenuInstance mainMenu)
        {
            this.application = application;
            this.assembly = applicationInstance.GetType().Assembly;
            this.applicationInstance = applicationInstance;
            this.mainMenu = mainMenu ?? new NullMainMenuInstance();

            B1ApplicationEventDispatcher = new B1ApplicationEventDispatcher();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B1EventsManager" /> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="applicationInstance">
        /// The instance responsible for application event handling.
        /// </param>
        public B1EventsManager(
            Application application,
            IApplicationInstance applicationInstance) : this(application, applicationInstance, null)
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

        private void SetMainMenu()
        {
            var listener = new MainMenuEventListener(mainMenu);
            listener.Subscribe();
            B1EventFilterManager.Include(BoEventTypes.et_MENU_CLICK, @"ALL_FORMS");
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

        private void AddEventSink(Type type)
        {
            var sink = type.CreateInstance<IRightClickEventSink>();
            B1RightClickEventDispatcher.AddListener(sink);
        }

        private void AddEventSinks()
        {
            var eventSinks = FindEventSinks();

            foreach (Type type in eventSinks)
            {
                AddEventSink(type);
            }
        }

        private void AddSystemFormLoadListener(Type classType)
        {
            var attribute = GetAttribute<B1SystemFormTypeAttribute>(classType);
            string formType = attribute.FormType;

            var loadListener = new SystemFormLoadListener(application, formType, classType);

            B1ItemEventDispatcher.AddListener(loadListener);
            B1EventFilterManager.Include(BoEventTypes.et_FORM_LOAD, formType);
        }

        private void AddSystemFormLoadListeners()
        {
            var systemForms = FindSystemFormsInAssembly();

            foreach (Type systemForm in systemForms)
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
                .Where(IsDecoratedWithSystemForm());

            return systemForms;
        }

        private T GetAttribute<T>(Type systemForm)
        {
            object attribute = systemForm.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            return (T) attribute;
        }

        private static Func<Type, bool> IsDecoratedWithSystemForm()
        {
            return t => t.GetCustomAttributes(typeof(B1SystemFormTypeAttribute), true).Any();
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
            B1ApplicationEventDispatcher.SetListener(applicationInstance);
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
    }
}
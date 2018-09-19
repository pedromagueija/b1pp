// <copyright filename="ApplicationEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ApplicationEvents
{
    using System;

    using SAPbouiCOM;

    internal class ApplicationEventDispatcher
    {
        private IApplicationEventsHandler eventsHandler;

        private Application Application { get; set; }

        public ApplicationEventDispatcher(IApplicationEventsHandler handler)
        {
            Ensure.NotNull(nameof(handler), handler);

            eventsHandler = handler;
        }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void Subscribe(Application application)
        {
            Application = application;

            Application.AppEvent -= OnAppEvent;
            Application.AppEvent += OnAppEvent;
        }

        public void Unsubscribe()
        {
            if (Application != null)
            {
                Application.AppEvent -= OnAppEvent;
            }
        }

        private void OnAppEvent(BoAppEventTypes eventType)
        {
            switch (eventType)
            {
                case BoAppEventTypes.aet_LanguageChanged:
                    SafeInvoke(eventsHandler.OnLanguageChanged);                        
                    break;
                case BoAppEventTypes.aet_CompanyChanged:
                    SafeInvoke(eventsHandler.OnCompanyChanged);
                    break;
                case BoAppEventTypes.aet_FontChanged:
                    SafeInvoke(eventsHandler.OnFontChanged);
                    break;
                case BoAppEventTypes.aet_ServerTerminition:
                    SafeInvoke(eventsHandler.OnAddonStopped);
                    break;
                case BoAppEventTypes.aet_ShutDown:
                    SafeInvoke(eventsHandler.OnShutdown);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }

        private void SafeInvoke(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                EventHandlerError(this, new ErrorEventArgs(e));
            }
        }
    }
}
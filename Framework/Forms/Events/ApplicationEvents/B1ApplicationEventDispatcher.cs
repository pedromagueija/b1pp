// <copyright filename="B1ApplicationEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.ApplicationEvents
{
    using System;

    using SAPbouiCOM;

    internal class B1ApplicationEventDispatcher : ISystemEventSubscriber
    {
        private IApplicationEventsHandler eventsHandler;

        private Application Application { get; set; }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void SetListener(IApplicationEventsHandler listener)
        {
            eventsHandler = listener;
        }

        public void Subscribe(Application application)
        {
            Application = application;
            Application.AppEvent += OnAppEvent;
        }

        public void Unsubscribe()
        {
            if (Application != null)
            {
                Application.AppEvent -= OnAppEvent;
                eventsHandler = null;
            }
        }

        private void OnAppEvent(BoAppEventTypes eventType)
        {
            try
            {
                if (eventsHandler == null)
                {
                    throw new InvalidOperationException("There is no listener for 'Application.AppEvent'.");
                }

                switch (eventType)
                {
                    case BoAppEventTypes.aet_LanguageChanged:
                        eventsHandler.OnLanguageChanged();
                        break;
                    case BoAppEventTypes.aet_CompanyChanged:
                        eventsHandler.OnCompanyChanged();
                        break;
                    case BoAppEventTypes.aet_FontChanged:
                        eventsHandler.OnFontChanged();
                        break;
                    case BoAppEventTypes.aet_ServerTerminition:
                        eventsHandler.OnAddonStopped();
                        break;
                    case BoAppEventTypes.aet_ShutDown:
                        eventsHandler.OnShutdown();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
                }
            }
            catch (Exception e)
            {
                EventHandlerError(null, new ErrorEventArgs(e));
            }
        }
    }
}
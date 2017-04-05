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
        private IApplicationInstance instance;

        private Application Application { get; set; }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void SetListener(IApplicationInstance listener)
        {
            instance = listener;
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
                instance = null;
            }
        }

        private void OnAppEvent(BoAppEventTypes eventType)
        {
            try
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("There is no listener for 'Application.AppEvent'.");
                }

                switch (eventType)
                {
                    case BoAppEventTypes.aet_LanguageChanged:
                        instance.OnLanguageChanged();
                        break;
                    case BoAppEventTypes.aet_CompanyChanged:
                        instance.OnCompanyChanged();
                        break;
                    case BoAppEventTypes.aet_FontChanged:
                        instance.OnFontChanged();
                        break;
                    case BoAppEventTypes.aet_ServerTerminition:
                        instance.OnAddonStopped();
                        break;
                    case BoAppEventTypes.aet_ShutDown:
                        instance.OnShutdown();
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
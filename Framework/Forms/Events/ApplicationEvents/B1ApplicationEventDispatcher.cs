// <copyright filename="B1ApplicationEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.ApplicationEvents
{
    using System;

    using SAPbouiCOM;

    internal static class B1ApplicationEventDispatcher
    {
        private static IApplicationInstance instance;

        private static Application Application { get; set; }

        public static event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public static void SetListener(IApplicationInstance listener)
        {
            instance = listener;
        }

        public static void Subscribe(Application application)
        {
            Application = application;
            Application.AppEvent += OnAppEvent;
        }

        public static void Unsubscribe()
        {
            if (Application != null)
            {
                Application.AppEvent -= OnAppEvent;
                instance = null;
            }
        }

        private static void OnAppEvent(BoAppEventTypes eventType)
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
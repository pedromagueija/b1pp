// <copyright filename="B1MenuEventDispatcher.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.MenuEvents
{
    using System;
    using System.Collections.Generic;

    using Extensions.SDK.UI;

    using SAPbouiCOM;

    internal static class B1MenuEventDispatcher
    {
        private static IMainMenuEventListener mainMenuEventListener;

        private static Dictionary<string, IFormMenuEventListener> menuEventListeners =
            new Dictionary<string, IFormMenuEventListener>();

        private static Application Application { get; set; }

        public static void AddListener(IFormMenuEventListener listener)
        {
            menuEventListeners.Add(listener.FormId, listener);
        }

        public static void AddMainMenuListener(IMainMenuEventListener listener)
        {
            mainMenuEventListener = listener;
        }

        public static event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public static void RemoveListener(IFormMenuEventListener listener)
        {
            menuEventListeners.Remove(listener.FormId);
        }

        public static void RemoveMainMenuListener()
        {
            mainMenuEventListener = null;
        }

        /// <summary>
        /// Subscribe to Menu Events from the application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="application"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when trying to call subscribe twice or more.
        /// </exception>
        public static void Subscribe(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (Application != null)
            {
                throw new InvalidOperationException("Cannot subscribe twice to 'Application.MenuEvent'.");
            }

            Application = application;
            Application.MenuEvent += OnMenuEvent;
        }

        public static void Unsubscribe()
        {
            if (Application != null)
            {
                Application.MenuEvent -= OnMenuEvent;
                Application = null;
                menuEventListeners = new Dictionary<string, IFormMenuEventListener>();
                RemoveMainMenuListener();
            }
        }

        private static bool IsFormMenu(string activeFormId)
        {
            return !string.IsNullOrEmpty(activeFormId) && menuEventListeners.ContainsKey(activeFormId);
        }

        private static void OnMenuEvent(ref MenuEvent e, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                string activeFormId = Application.GetActiveFormId();
                if (IsFormMenu(activeFormId))
                {
                    bool handled = menuEventListeners[activeFormId].OnMenuEvent(ref e, out bubbleEvent);
                    if (handled)
                        return;
                }

                mainMenuEventListener?.OnMenuEvent(ref e, out bubbleEvent);
            }
            catch (Exception exception)
            {
                EventHandlerError(null, new ErrorEventArgs(exception));
            }
        }
    }
}
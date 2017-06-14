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

    internal class B1MenuEventDispatcher
    {
        private IMainMenuEventListener mainMenuEventListener;

        private Dictionary<string, IFormMenuEventListener> menuEventListeners =
            new Dictionary<string, IFormMenuEventListener>();

        private Application Application { get; set; }

        public void AddListener(IFormMenuEventListener listener)
        {
            menuEventListeners.Add(listener.FormId, listener);
        }

        public void AddMainMenuListener(IMainMenuEventListener listener)
        {
            mainMenuEventListener = listener;
        }

        public event EventHandler<ErrorEventArgs> EventHandlerError = delegate { };

        public void RemoveListener(IFormMenuEventListener listener)
        {
            menuEventListeners.Remove(listener.FormId);
        }

        public void RemoveMainMenuListener()
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
        public void Subscribe(Application application)
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

        public void Unsubscribe()
        {
            if (Application != null)
            {
                Application.MenuEvent -= OnMenuEvent;
                Application = null;
                menuEventListeners = new Dictionary<string, IFormMenuEventListener>();
                RemoveMainMenuListener();
            }
        }

        private bool IsFormMenu(string activeFormId)
        {
            return !string.IsNullOrEmpty(activeFormId) && menuEventListeners.ContainsKey(activeFormId);
        }

        private void OnMenuEvent(ref MenuEvent e, out bool bubbleEvent)
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
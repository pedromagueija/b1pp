// <copyright filename="B1EventFilterManager.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using SAPbouiCOM;

    /// <summary>
    /// Manage what events that are processed by the addon, by setting filters on sent events.
    /// <para />
    /// It is recommended that addons only process (and receive) events that it handles,
    /// <para />
    /// and all other events should be filtered out.
    /// </summary>
    internal static class B1EventFilterManager
    {
        private static Application app;

        /// <summary>
        /// Helper to handle the xml filter format.
        /// </summary>
        private static readonly XmlEventFilter XmlFilter = new XmlEventFilter();

        /// <summary>
        /// Includes the specified event type on the sending list.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formType">Type of the form.</param>
        public static void Include(BoEventTypes eventType, string formType)
        {
            if (XmlFilter.IsEventTypeMissing(eventType))
            {
                XmlFilter.AddEventType(eventType);
            }

            if (XmlFilter.IsFormTypeMissing(eventType, formType))
            {
                XmlFilter.AddFormTypeToEvent(eventType, formType);
                SetEventSending();
            }
        }

        /// <summary>
        /// Initializes the event filter manager. This is done by disabling all events.
        /// </summary>
        public static void Initialize(Application application)
        {
            app = application;
            DisableEventSending();
            XmlFilter.Load(application.GetFilter().GetAsXML());
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public new static string ToString()
        {
            return XmlFilter.ToString();
        }

        /// <summary>
        /// Disables the event sending. After calling this the addon should not receive any events,
        /// <para />
        /// except for Application events.
        /// </summary>
        private static void DisableEventSending()
        {
            app.SetFilter();
        }

        /// <summary>
        /// Sets the event sending with the included events, if any.
        /// </summary>
        private static void SetEventSending()
        {
            EventFilters eventFilters = app.GetFilter();
            eventFilters.LoadFromXML(XmlFilter.ToString());
            app.SetFilter(eventFilters);
        }
    }
}
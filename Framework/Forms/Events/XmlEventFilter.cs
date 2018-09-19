// <copyright filename="XmlEventFilter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using SAPbouiCOM;

    /// <summary>
    /// Helper to produce the Xml format needed by the event filter in SAP Business One.
    /// </summary>
    internal class XmlEventFilter
    {
        /// <summary>
        /// Represents the xml file to be produced.
        /// </summary>
        private XDocument xmlEventsStructure = new XDocument();

        /// <summary>
        /// Adds a new event tag to the events tag with the given event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        public void AddEventType(BoEventTypes eventType)
        {
            var events = xmlEventsStructure.Descendants("events").First();
            events.Add(CreateEventOfType(eventType));
        }

        /// <summary>
        /// Adds a new form tag to the forms tag with the given form type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formType">Type of the form.</param>
        public void AddFormTypeToEvent(BoEventTypes eventType, string formType)
        {
            var eventElement = FindEventElement(eventType);
            var formsElement = CreateOrGet(eventElement, "forms");
            var formTypeElement = new XElement("form");
            var formTypeAttribute = new XAttribute("form_id", formType);

            formTypeElement.Add(formTypeAttribute);
            formsElement.Add(formTypeElement);
        }

        /// <summary>
        /// Determines whether an event with the given type exists.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>True when an event with the given type exists, false otherwise.</returns>
        public bool IsEventTypeMissing(BoEventTypes eventType)
        {
            var allEvents = xmlEventsStructure.Descendants("event");
            return !allEvents.Any(WithType(eventType));
        }

        /// <summary>
        /// Determines whether a form with the given type already exists.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formType">Type of the form.</param>
        /// <returns>True when a form with the type exists, false otherwise.</returns>
        public bool IsFormTypeMissing(BoEventTypes eventType, string formType)
        {
            var eventElement = FindEventElement(eventType);
            return !eventElement.Descendants("form").Any(WithFormType(formType));
        }

        /// <summary>
        /// Loads the specified event filters.
        /// </summary>
        /// <param name="eventFilters">The event filters.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when eventFilters is null or empty.
        /// </exception>
        public void Load(string eventFilters)
        {
            if (string.IsNullOrEmpty(eventFilters))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(eventFilters));
            }

            xmlEventsStructure = XDocument.Parse(eventFilters);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return xmlEventsStructure.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Creates an event tag with the given type on the type attribute.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>An <see cref="XElement" /> that represents the created tag.</returns>
        private XElement CreateEventOfType(BoEventTypes eventType)
        {
            var typeAttribute = new XAttribute("type", EventTypeAsString(eventType));
            return new XElement("event", typeAttribute);
        }

        private XElement CreateOrGet(XElement eventElement, string name)
        {
            var descendants = eventElement.Descendants(name);
            if (!descendants.Any())
            {
                eventElement.Add(new XElement(name));
            }

            return eventElement.Element(name);
        }

        /// <summary>
        /// Converts the given event type to a string.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>A string that represents the given event type.</returns>
        private string EventTypeAsString(BoEventTypes eventType)
        {
            return ((int) eventType).ToString();
        }

        /// <summary>
        /// Finds the event element with the type attribute that matches <paramref name="eventType" />.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>The <see cref="XElement" /> that represents the found event.</returns>
        private XElement FindEventElement(BoEventTypes eventType)
        {
            return xmlEventsStructure.Descendants("event").First(WithType(eventType));
        }

        /// <summary>
        /// Checks if the attribute matches the given type.
        /// </summary>
        /// <param name="formType">Type of the form.</param>
        /// <returns>True when it matches, false otherwise.</returns>
        private Func<XElement, bool> WithFormType(string formType)
        {
            return e => e.Attribute("form_id").Value == formType || e.Attribute("form_id").Value == "ALL_FORMS";
        }

        /// <summary>
        /// Checks if the attribute matches the given type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>True when it matches, false otherwise.</returns>
        private Func<XElement, bool> WithType(BoEventTypes eventType)
        {
            return element => element.Attribute("type").Value == EventTypeAsString(eventType);
        }
    }
}
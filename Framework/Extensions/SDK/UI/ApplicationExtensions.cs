// <copyright filename="ApplicationExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbouiCOM;

    /// <summary>
    /// Common application object actions.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Applies the specified XML.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="xml">The XML.</param>
        public static void Apply(this Application application, string xml)
        {
            application.LoadBatchActions(ref xml);
        }

        /// <summary>
        /// Creates an instance of the object. This method casts the object to the given T type.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="application">The application.</param>
        /// <param name="type">The enum type of the object we wish to create.</param>
        /// <returns>An instance of the given object.</returns>
        public static T Create<T>(this Application application, BoCreatableObjectType type)
        {
            return (T) application.CreateObject(type);
        }

        /// <summary>
        /// Creates a form using the xml document and unique id provided.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="formType">The form type identifier. Must not be empty or null.</param>
        /// <param name="formUniqueId">The form unique identifier. Must not be empty or null.</param>
        /// <param name="formXml">The form xml definition. Must not be empty or null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="formType" />, <paramref name="formUniqueId" /> or <paramref name="formXml" /> are null or
        /// empty.
        /// </exception>
        /// <returns>The created form.</returns>
        public static Form CreateForm(this Application application, string formType, string formUniqueId,
            string formXml)
        {
            if (string.IsNullOrEmpty(formType) || string.IsNullOrEmpty(formUniqueId) || string.IsNullOrEmpty(formXml))
            {
                throw new ArgumentNullException("FormType, FormUniqueId and FormXml cannot be null or empty.",
                    (Exception) null);
            }

            var fcp = application.Create<FormCreationParams>(BoCreatableObjectType.cot_FormCreationParams);

            fcp.UniqueID = formUniqueId;
            fcp.FormType = formType;
            fcp.XmlData = formXml;

            return application.Forms.AddEx(fcp);
        }

        /// <summary>
        /// Gets the active form identifier.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>
        /// The active form unique id, or an empty string.
        /// </returns>
        public static string GetActiveFormId(this Application application)
        {
            try
            {
                var activeForm = application.Forms.ActiveForm;
                return activeForm?.UniqueID ?? string.Empty;
            }
            catch (COMException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the Business One event filters from XML.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="xmlFilters">The XML filters.</param>
        public static void SetFilterFromXml(this Application application, string xmlFilters)
        {
            var eventFilters = application.GetFilter();
            eventFilters.LoadFromXML(xmlFilters);
            application.SetFilter(eventFilters);
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="message">The message.</param>
        public static void ShowError(this Application application, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            application.StatusBar.SetSystemMessage(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
        }

        /// <summary>
        /// Shows a information status bar message. Empty messages are ignored.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="message">The message.</param>
        /// <remarks>A message containing only whitespace characters is considered empty.</remarks>
        public static void ShowInfo(this Application application, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            application.StatusBar.SetSystemMessage(message, BoMessageTime.bmt_Short,
                BoStatusBarMessageType.smt_Warning);
        }
    }
}
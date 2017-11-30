// <copyright filename="IApplicationInstance.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.ApplicationEvents
{
    /// <summary>
    /// Standard application events from SAP Business One.
    /// </summary>
    public interface IApplicationEventsHandler
    {
        /// <summary>
        /// Called when the addon is requested to stop by SAP Business One.
        /// </summary>
        void OnAddonStopped();

        /// <summary>
        /// Called when the company is changed in SAP Business One.
        /// </summary>
        void OnCompanyChanged();

        /// <summary>
        /// Called when the font is changed in SAP Business One.
        /// </summary>
        void OnFontChanged();

        /// <summary>
        /// Called when the language is changed in SAP Business One.
        /// </summary>
        void OnLanguageChanged();

        /// <summary>
        /// Called when SAP Business One is closed.
        /// </summary>
        void OnShutdown();
    }
}
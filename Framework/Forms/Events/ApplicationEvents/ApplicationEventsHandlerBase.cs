// <copyright filename="ApplicationEventsHandlerBase.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ApplicationEvents
{
    using B1PP;

    /// <summary>
    /// Base for application events handlers.
    /// </summary>
    public abstract class ApplicationEventsHandlerBase : IApplicationEventsHandler
    {
        private readonly IAddon addon;

        /// <summary>
        /// Initializes the base class with the addon instance to use.
        /// </summary>
        /// <param name="addon">
        /// The addon to use.
        /// </param>
        protected ApplicationEventsHandlerBase(IAddon addon)
        {
            this.addon = addon;
        }

        /// <summary>
        /// Called when the addon is requested to stop by SAP Business One.
        /// </summary>
        public virtual void OnAddonStopped()
        {
            addon.Exit();
        }

        /// <summary>
        /// Called when the company is changed in SAP Business One.
        /// </summary>
        public virtual void OnCompanyChanged()
        {
            addon.Restart();
        }

        /// <summary>
        /// Called when the font is changed in SAP Business One.
        /// </summary>
        public virtual void OnFontChanged()
        {
        }

        /// <summary>
        /// Called when the language is changed in SAP Business One.
        /// </summary>
        public virtual void OnLanguageChanged()
        {
        }

        /// <summary>
        /// Called when SAP Business One is closed.
        /// </summary>
        public virtual void OnShutdown()
        {
            addon.Exit();
        }
    }
}
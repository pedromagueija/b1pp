// <copyright filename="UserForm.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms
{
    using Events;
    using Events.ItemEvents;
    using SAPbouiCOM;

    /// <summary>
    /// Represents a user form.
    /// </summary>
    public abstract class UserForm : IFormInstance
    {
        /// <summary>
        /// The application this form is associated with.
        /// </summary>
        protected Application Application { get; }

        /// <summary>
        /// Database data sources on this form.
        /// </summary>
        protected DBDataSources DbDataSources { get; private set; }

        /// <summary>
        /// Event manager.
        /// </summary>
        protected EventListener Events { get; private set; }
        
        /// <summary>
        /// The SAP Business One <see cref="SAPbouiCOM.Form"/> object.
        /// </summary>
        protected Form Form { get; private set; }

        /// <summary>
        /// User data sources on this form.
        /// </summary>
        protected UserDataSources UserDataSources { get; private set; }

        /// <summary>
        /// Form id.
        /// </summary>
        public string FormId { get; private set; }
        
        /// <summary>
        /// Form type.
        /// </summary>
        public string FormType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserForm"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        protected UserForm(Application application)
        {
            Application = application;
        }

        /// <summary>
        /// Loads this form instance.
        /// </summary>
        public virtual void Load()
        {
            var loader = new UserFormLoader(Application, this);
            Form = loader.Load();
            FormId = Form.UniqueID;
            FormType = Form.TypeEx;
            DbDataSources = Form.DataSources.DBDataSources;
            UserDataSources = Form.DataSources.UserDataSources;

            Initialize();
        }

        /// <summary>
        /// Initializes this form instance.
        /// When overriden you must call base.Initialize() or setup the Events yourself.
        /// </summary>
        protected virtual void Initialize()
        {
            Events = new EventListener(this);
            Events.Subscribe();
        }

        /// <summary>
        /// Handles the form close event. In particular removes any event listeners associated with this form.
        /// </summary>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        [ItemEventHandler(BoEventTypes.et_FORM_CLOSE)]
        protected virtual void OnAfterFormClose(ItemEvent e)
        {
            Events.Unsubscribe();
        }
    }
}
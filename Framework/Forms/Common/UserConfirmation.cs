// <copyright filename="UserConfirmation.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Common
{
    using SAPbouiCOM;

    /// <summary>
    /// Display the given message on-screen and prompts the user for a choice.
    /// </summary>
    public sealed class UserConfirmation
    {
        private readonly Application application;
        private readonly string message;
        private readonly string defaultButtonCaption;
        private readonly string alternateButtonCaption;

        /// <summary>
        /// Creates a new instance of <see cref="UserConfirmation"/>.
        /// </summary>
        /// <param name="application">The application where the message is to be displayed.</param>
        /// <param name="message">The message to display.</param>
        public UserConfirmation(Application application, string message) : this(application, message, @"Yes", @"No")
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="UserConfirmation"/>.
        /// </summary>
        /// <param name="application">The application where the message is to be displayed.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="defaultButtonCaption">The caption for the default button.</param>
        /// <param name="alternateButtonCaption">The caption for the alternate button.</param>
        public UserConfirmation(Application application, string message, string defaultButtonCaption, string alternateButtonCaption)
        {
            this.application = application;
            this.message = message;
            this.defaultButtonCaption = defaultButtonCaption;
            this.alternateButtonCaption = alternateButtonCaption;
        }

        /// <summary>
        /// Displays the prompt on-screen.
        /// </summary>
        /// <returns>
        /// True if the user has confirmed, false otherwise.
        /// </returns>
        /// <remark>
        /// We consider as confirmed when the button pressed equals the default button.
        /// </remark>
        public bool Execute()
        {
            return application.MessageBox(message, 1, defaultButtonCaption, alternateButtonCaption) == 1;
        }
    }
}
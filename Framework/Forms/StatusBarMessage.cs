// <copyright filename="StatusBarMessage.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms
{
    using SAPbouiCOM;

    /// <summary>
    /// Displays a status bar message as error, warning or success,
    /// with the given message and parameters.
    /// </summary>
    /// <example>
    ///     <code>
    /// var message = new StatusBarMessage(application, @"Hello {0}!", "World");
    /// </code>
    /// </example>
    public class StatusBarMessage
    {
        private readonly Application application;
        private readonly object[] args;
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBarMessage" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="args">The arguments for the text.</param>
        public StatusBarMessage(Application application, string text, params object[] args)
        {
            this.application = application;
            this.args = args;
            this.text = text ?? string.Empty;
        }

        /// <summary>
        /// Displays the message as error.
        /// </summary>
        public void Error()
        {
            DisplayMessage(BoStatusBarMessageType.smt_Error);
        }

        /// <summary>
        /// Displays the message as success.
        /// </summary>
        public void Success()
        {
            DisplayMessage(BoStatusBarMessageType.smt_Success);
        }

        /// <summary>
        /// Displays the message as warning.
        /// </summary>
        public void Warning()
        {
            DisplayMessage(BoStatusBarMessageType.smt_Warning);
        }

        private void DisplayMessage(BoStatusBarMessageType messageType)
        {
            string message = string.Format(text, args);
            application.StatusBar.SetSystemMessage(message, BoMessageTime.bmt_Short, messageType);
        }
    }
}
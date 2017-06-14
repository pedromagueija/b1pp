namespace B1PP.Forms.Events.ApplicationEvents
{
    using System;

    using SAPbouiCOM;

    /// <summary>
    /// Represents a system event subscriber that subscribes to events
    /// <para/>
    /// from the <see cref="Application"/> object.
    /// </summary>
    internal interface ISystemEventSubscriber
    {
        /// <summary>
        /// Raised when an event handling results in an error.
        /// </summary>
        event EventHandler<ErrorEventArgs> EventHandlerError;

        /// <summary>
        /// Subscribes the specified application events.
        /// </summary>
        /// <param name="application">The application.</param>
        void Subscribe(Application application);

        /// <summary>
        /// Unsubscribes this instance.
        /// </summary>
        void Unsubscribe();
    }
}
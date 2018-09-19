// <copyright filename="IEventFilterManager.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using SAPbouiCOM;

    /// <summary>
    /// Allows adding filters to the SAP Business One Event Filter (improves addon performance).
    /// </summary>
    public interface IEventFilterManager
    {
        /// <summary>
        /// Includes the specified event type on the sending list.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formType">Type of the form.</param>
        void Include(BoEventTypes eventType, string formType);
    }
}
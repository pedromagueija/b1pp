// <copyright filename="IFilterable.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;

    /// <summary>
    /// Marks a type as being eligible for SAP Business One event filtering.
    /// </summary>
    /// <remarks>
    /// Filtering is done by raising the HandlerAdded event, every time an SAP Business One
    /// <para />
    /// event handler is added.
    /// </remarks>
    internal interface IFilterable
    {
        /// <summary>
        /// Occurs when an SAP Business One event handler is added.
        /// </summary>
        event EventHandler<HandlerAddedEventArgs> HandlerAdded;
    }
}
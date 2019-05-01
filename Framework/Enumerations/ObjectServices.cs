// <copyright filename="ObjectServices.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Enumerations
{
    /// <summary>
    /// Services provided by the User Defined Object.
    /// </summary>
    public enum ObjectServices
    {
        /// <summary>
        /// The find service allows to find records.
        /// </summary>
        Find,

        /// <summary>
        /// The delete service allows removal of records.
        /// </summary>
        Delete,

        /// <summary>
        /// The cancel service allows marking of a record as canceled.
        /// </summary>
        Cancel,

        /// <summary>
        /// The close service allows marking of a record as closed.
        /// </summary>
        Close,

        /// <summary>
        /// The log service allows auditing of changes to records.
        /// </summary>
        Log,

        /// <summary>
        /// The manage series service allows series to be managed within SAP Business One.
        /// </summary>
        ManageSeries,

        /// <summary>
        /// The year transfer service allows year transfer copying.
        /// </summary>
        YearTransfer,

        /// <summary>
        /// Default selection of services (Find and Delete).
        /// </summary>
        Default
    }
}
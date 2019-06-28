// <copyright filename="IAddMasterData.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database.Commands
{
    /// <summary>
    /// Add a master data type record to the database.
    /// </summary>
    /// <typeparam name="T">
    /// The type of master data record.
    /// </typeparam>
    public interface IAddMasterData<in T> where T : IMasterDataRecord
    {
        /// <summary>
        /// True when the action completes with success, false otherwise.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Contains the error, if any, that caused the action to fail.
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="instance">
        /// The instance of master data record to add.
        /// </param>
        /// <returns>
        /// The key of the added record.
        /// </returns>
        string Execute(T instance);
    }
}
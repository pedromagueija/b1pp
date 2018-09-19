// <copyright filename="IAddon.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP
{
    /// <summary>
    /// Represents an add-on.
    /// </summary>
    public interface IAddon
    {
        /// <summary>
        /// Stops and exits the add-on process.
        /// </summary>
        void Exit();

        /// <summary>
        /// Restarts the add-on.
        /// </summary>
        void Restart();


        /// <summary>
        /// Starts the add-on.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the add-on without exiting the process.
        /// </summary>
        void Stop();
    }
}
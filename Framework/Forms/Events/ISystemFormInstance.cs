// <copyright filename="ISystemFormInstance.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    /// <summary>
    /// Represents a system form instance.
    /// </summary>
    /// <seealso cref="IFormInstance" />
    public interface ISystemFormInstance : IFormInstance
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <remarks>
        /// Use this method to register any events you may need.
        /// </remarks>
        void Initialize();
    }
}
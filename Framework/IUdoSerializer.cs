// <copyright filename="IUdoSerializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP
{
    /// <summary>
    /// Serializes user-defined-objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of user-defined-object.
    /// </typeparam>
    public interface IUdoSerializer<T> where T : class
    {
        /// <summary>
        /// Serialize the given instance.
        /// </summary>
        /// <param name="instance">
        /// The instance to serialize.
        /// </param>
        /// <returns>
        /// A string with the XML serialized object.
        /// </returns>
        string Serialize(T instance);
    }
}
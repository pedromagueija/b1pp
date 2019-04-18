// <copyright filename="IUdoDeserializer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP
{
    /// <summary>
    /// Deserializes a user-defined-object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the user-defined-object.
    /// </typeparam>
    public interface IUdoDeserializer<out T> where T : class, new()
    {
        /// <summary>
        /// Deserialize the given XML into an object.
        /// </summary>
        /// <param name="xml">
        /// The XML of the serialized object.
        /// </param>
        /// <returns>
        /// An instance of the object.
        /// </returns>
        T Deserialize(string xml);
    }
}
// <copyright filename="InstanceCreatorDelegate.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    /// <summary>
    /// Creates an instance of a type from a given recordset reader.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="reader">The reader.</param>
    /// <returns>
    /// The type with the data from the reader.
    /// </returns>
    public delegate T InstanceCreator<out T>(IRecordsetReader reader);
}
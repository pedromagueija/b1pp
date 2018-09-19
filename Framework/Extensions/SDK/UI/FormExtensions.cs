// <copyright filename="FormExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using SAPbouiCOM;

    /// <summary>
    /// Common and helpful for <see cref="Form" />.
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// Gets a specific item from the items collection.
        /// </summary>
        /// <typeparam name="T">The type to convert the item to.</typeparam>
        /// <param name="form">The form that contains the item.</param>
        /// <param name="itemId">The id of the item.</param>
        /// <returns>The specific item.</returns>
        /// <exception cref="InvalidSpecificItemTypeException">Thrown when the item cannot be cast to the given type.</exception>
        /// <exception cref="ItemNotFoundException">Thrown when the item is not found in the collection.</exception>
        public static T Get<T>(this Form form, string itemId) where T : class
        {
            return form.Items.Get<T>(itemId);
        }
    }
}
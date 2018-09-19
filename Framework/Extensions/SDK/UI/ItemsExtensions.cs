// <copyright filename="ItemsExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Runtime.InteropServices;

    using SAPbouiCOM;

    public static class ItemsExtensions
    {
        /// <summary>
        /// Gets a specific item from the items collection.
        /// </summary>
        /// <typeparam name="T">The type to convert the item to.</typeparam>
        /// <param name="items">The items collection.</param>
        /// <param name="itemId">The id of the item.</param>
        /// <returns>The specific item.</returns>
        /// <exception cref="InvalidSpecificItemTypeException">Thrown when the item cannot be cast to the given type.</exception>
        /// <exception cref="ItemNotFoundException">Thrown when the item is not found in the collection.</exception>
        public static T Get<T>(this Items items, string itemId) where T : class
        {
            Item item;

            try
            {
                item = items.Item(itemId);
            }
            catch (COMException ex)
            {
                var message = $"Item '{itemId}' was not found";
                throw new ItemNotFoundException(message, ex);
            }

            if (typeof(T) == typeof(Item))
            {
                return (T) item;
            }

            var specificItem = item.Specific as T;
            if (specificItem != null)
            {
                return specificItem;
            }

            throw CreateInvalidSpecificItemTypeException(itemId, item, typeof(T));
        }

        private static InvalidSpecificItemTypeException CreateInvalidSpecificItemTypeException(
            string itemId,
            Item item,
            Type type)
        {
            var message = $"Item '{itemId}' is not of type '{type}'.";
            var exception = new InvalidSpecificItemTypeException(message);
            exception.Data.Add("itemId", itemId);
            exception.Data.Add("item", item.Type.ToString());
            exception.Data.Add("t", type);
            return exception;
        }
    }
}
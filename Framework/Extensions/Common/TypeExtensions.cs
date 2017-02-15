// <copyright filename="TypeExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.Common
{
    using System;
    using System.Linq;

    /// <summary>
    /// Common type extension methods.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if a type implements the given interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>
        /// True if the type implements the interface, false otherwise.
        /// </returns>
        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        /// <summary>
        /// Creates an instance of the current type and casts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type to cast the new instance to.
        /// </typeparam>
        /// <param name="type">The current type.</param>
        /// <returns>
        /// A new instance of the given type cast to the given <typeparamref name="T"/> type.
        /// </returns>
        public static T CreateInstance<T>(this Type type)
        {
            return (T) Activator.CreateInstance(type);
        }
    }
}
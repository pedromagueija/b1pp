// <copyright filename="ClassHelper.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class ClassHelper
    {
        /// <summary>
        /// The eligible methods traits.
        /// </summary>
        private const BindingFlags EligibleMethods =
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Finds methods annotated with the given attribute on the given instance.
        /// </summary>
        /// <typeparam name="T">Type of the attribute.</typeparam>
        /// <param name="instance">The instance to search.</param>
        /// <returns>
        /// An enumeration of the methods with the attribute.
        /// </returns>
        public static IEnumerable<MethodInfo> FindAnnotatedMethods<T>(object instance)
        {
            return instance.GetType().GetMethods(EligibleMethods).Where(HasAttribute<T>());
        }

        /// <summary>
        /// Determines whether this instance has attribute T.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <returns>
        /// True when the method is annotated with the T attribute, false otherwise.
        /// </returns>
        private static Func<MethodInfo, bool> HasAttribute<T>()
        {
            return m => m.GetCustomAttributes(typeof(T), true).Any();
        }
    }
}
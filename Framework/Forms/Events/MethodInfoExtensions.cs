// <copyright filename="MethodInfoExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension methods that provide useful utilities in the SAP Business One context.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Creates an after event delegate using the method information provided and the object instance.
        /// </summary>
        /// <param name="method">The method information to be used as a delegate.</param>
        /// <param name="instance">The object instance.</param>
        /// <returns>
        /// An after event delegate.
        /// </returns>
        public static Action CreateAfterEventDelegate(this MethodInfo method, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return (Action) Delegate.CreateDelegate(typeof(Action), instance, method.Name, true);
        }

        /// <summary>
        /// Creates an after event delegate using the method information provided and the object instance.
        /// <para />
        /// The T type identifies the arguments of the method.
        /// </summary>
        /// <param name="method">The method information to be used as a delegate.</param>
        /// <param name="instance">The object instance.</param>
        /// <returns>
        /// An after event delegate.
        /// </returns>
        public static Action<T> CreateAfterEventDelegate<T>(this MethodInfo method, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return (Action<T>) Delegate.CreateDelegate(typeof(Action<T>), instance, method.Name, true);
        }

        /// <summary>
        /// Creates a before event delegate using the method information provided and the object instance.
        /// <para />
        /// The T type identifies the arguments of the method. The boolean is the return type of the delegate.
        /// </summary>
        /// <param name="method">The method information to be used as a delegate.</param>
        /// <param name="instance">The object instance.</param>
        /// <returns>
        /// A before event delegate.
        /// </returns>
        public static Func<T, bool> CreateBeforeEventDelegate<T>(this MethodInfo method, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return (Func<T, bool>) Delegate.CreateDelegate(typeof(Func<T, bool>), instance, method.Name, true);
        }

        /// <summary>
        /// Gets a specific attribute from a method (provided it has one).
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="method">The method information to extract the attribute from.</param>
        /// <returns>
        /// The attribute cast to the given attribute type.
        /// </returns>
        public static T GetAttribute<T>(this MethodInfo method) where T : Attribute
        {
            return (T) method.GetCustomAttributes(typeof(T), true).FirstOrDefault(m => m is T);
        }
    }
}
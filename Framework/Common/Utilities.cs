// <copyright filename="Utilities.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Common
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Common utilities used in the project.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Releases the COM objects.
        /// </summary>
        /// <param name="objects">COM objects to release.</param>
        public static void Release(params object[] objects)
        {
            foreach (object obj in objects)
            {
                ReleaseOne(obj);
            }
        }

        /// <summary>
        /// Checks if the object is a COM object.
        /// </summary>
        /// <param name="o">The object to check.</param>
        /// <returns>True if the object is a COM object, false otherwise.</returns>
        private static bool NotComObj(object o)
        {
            return !"System.__ComObject".Equals(o.GetType().ToString());
        }

        /// <summary>
        /// Releases the COM object.
        /// </summary>
        /// <param name="o">The object to release.</param>
        private static void ReleaseOne(object o)
        {
            if (o == null || NotComObj(o))
            {
                return;
            }

            Marshal.ReleaseComObject(o);
        }
    }
}
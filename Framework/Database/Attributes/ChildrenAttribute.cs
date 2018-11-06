// <copyright filename="ChildrenAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    /// <summary>
    /// Marks a member as being a relationship to a child table.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ChildrenAttribute : Attribute
    {
        public Type Type { get; }

        public ChildrenAttribute(Type type)
        {
            Type = type;
        }
    }
}
// <copyright filename="SystemFieldAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;

    /// <summary>
    /// Marks a property or field as a system field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SystemFieldAttribute : Attribute
    {
    }
}
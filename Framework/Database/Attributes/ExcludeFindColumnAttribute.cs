// <copyright filename="ExcludeFindColumnAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    /// <summary>
    /// Skips marked members as being FindColumns on a user defined object.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExcludeFindColumnAttribute : Attribute
    {
    }
}
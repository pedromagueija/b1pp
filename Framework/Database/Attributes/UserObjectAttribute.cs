// <copyright filename="UserObjectAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    /// <summary>
    /// Marks a type as being linked with a User Defined Object.
    /// </summary>
    /// <remarks>
    /// Using spaces or other special characters in the name of the object can affect the ability to
    /// serialize for GeneralData. Due to this implication, spaces are replaced with underscores in the name.
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UserObjectAttribute : Attribute
    {
        /// <summary>
        /// The user-defined-object id.
        /// </summary>
        public string ObjectId { get; }
        
        /// <summary>
        /// The user-defined-object name.
        /// </summary>
        public string ObjectName { get; }
        
        /// <summary>
        /// The user-defined-object type.
        /// </summary>
        public BoUDOObjType ObjectType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="UserObjectAttribute"/>.
        /// </summary>
        /// <param name="objectId">The user-defined-object id.</param>
        /// <param name="objectName">The user-defined-object name.</param>
        /// <param name="objectType">The user-defined-object type.</param>
        public UserObjectAttribute(
            string objectId,
            string objectName,
            BoUDOObjType objectType)
        {
            // at the time of writing user-objects with spaces in their name
            // cannot be serialized, to avoid this we replace the spaces in the original name
            // with an '_'
            string userObjectName = objectName.Replace(@" ", @"_");

            ObjectId = objectId;
            ObjectName = userObjectName;
            ObjectType = objectType;
        }

        internal void Apply(UserObjectsMD userObject, string tableName)
        {
            userObject.Code = ObjectId;
            userObject.Name = ObjectName;
            userObject.ObjectType = ObjectType;
            userObject.TableName = tableName;
        }
    }
}
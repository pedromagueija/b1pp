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
    /// serialize for GeneralData. Due to this implication, spaces are removed from the name.
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class UserObjectAttribute : Attribute
    {
        public string ObjectId { get; }
        public string ObjectName { get; }
        public BoUDOObjType ObjectType { get; }

        public UserObjectAttribute(
            string objectId,
            string objectName,
            BoUDOObjType objectType)
        {
            ObjectId = objectId;
            ObjectName = objectName.Replace(" ", "");
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
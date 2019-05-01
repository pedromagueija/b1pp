// <copyright filename="UserFieldAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database.Attributes
{
    using System;

    using Extensions.Common;

    using SAPbobsCOM;

    /// <summary>
    /// Marks a property or field as a user field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class UserFieldAttribute : Attribute
    {
        /// <summary>
        /// The maximum alpha type size.
        /// </summary>
        private const int MaxAlphaTypeSize = 254;

        /// <summary>
        /// The maximum numeric type size.
        /// </summary>
        private const int MaxNumericTypeSize = 254;

        /// <summary>
        /// The default alpha type size.
        /// </summary>
        private const int DefaultAlphaTypeSize = 254;

        /// <summary>
        /// The default numeric type size.
        /// </summary>
        private const int DefaultNumericTypeSize = 11;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public BoFieldTypes Type { get; }

        /// <summary>
        /// Gets the subtype.
        /// </summary>
        /// <value>
        /// The subtype.
        /// </value>
        public BoFldSubTypes SubType { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size. Relevant only for Alpha and Numeric types.
        /// <para />
        /// Valid sizes are:
        /// <para />
        /// Alpha [1..254]
        /// <para />
        /// Numeric [1..11]
        /// </value>
        public int Size { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the size provided for the type is invalid.
        /// <para />
        /// Valid sizes are:
        /// <para />
        /// Alpha [1..254]
        /// <para />
        /// Numeric [1..11]
        /// </exception>
        public UserFieldAttribute(BoFieldTypes type, BoFldSubTypes subtype) : this(type, subtype, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the size provided for the type is invalid.
        /// <para />
        /// Valid sizes are:
        /// <para />
        /// Alpha [1..254]
        /// <para />
        /// Numeric [1..11]
        /// </exception>
        public UserFieldAttribute(BoFieldTypes type, BoFldSubTypes subtype, int size)
        {
            Type = type;
            SubType = subtype;
            Size = size;
        }

        /// <summary>
        /// Applies the type, subtype and size to the user field.
        /// </summary>
        /// <param name="field">The field.</param>
        internal void Apply(UserFieldsMD field)
        {
            field.Type = Type;
            field.SubType = SubType;

            if (Type == BoFieldTypes.db_Alpha)
            {
                field.EditSize = Size.InRange(1, MaxAlphaTypeSize) ? Size : DefaultAlphaTypeSize;
            }

            if (Type == BoFieldTypes.db_Numeric)
            {
                field.EditSize = Size.InRange(1, MaxNumericTypeSize) ? Size : DefaultNumericTypeSize;
            }
        }
    }
}
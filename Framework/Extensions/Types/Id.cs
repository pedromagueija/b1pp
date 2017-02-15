// <copyright filename="Id.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.Types
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an integer id of a record in SAP Business One.
    /// </summary>
    /// <remarks>
    /// This class facilitates the int/string duality present in B1. Sometimes an integer is prefered,
    /// sometimes a string is prefered. This object can be implicit converted to and from strings or ints.
    /// </remarks>
    [TypeConverter(typeof(IdTypeConverter))]
    public class Id
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Id"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the string does not represent a valid int.
        /// </exception>
        public Id(string value)
        {
            int v;
            if (int.TryParse(value, out v))
            {
                Value = v;
            }
            else
            {
                throw new ArgumentException($@"'{value}' is not an integer.", nameof(value));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Id"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Id(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Id(string id)
        {
            return new Id(id);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="Id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Id(int id)
        {
            return new Id(id);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Id"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [NotNull]
        public static implicit operator string([CanBeNull] Id id)
        {
            if (id == null)
                return string.Empty;

            return id.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Id"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(Id id)
        {
            return id.Value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance
        /// by calling the implicit string conversion.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // calls on the implicit conversion to string
            return this;
        }
    }
}
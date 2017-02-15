// <copyright filename="IdTypeConverter.cs" project="Framework">
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
    /// Standard type converter for the <see cref="Id" /> type.
    /// </summary>
    /// <seealso cref="System.ComponentModel.TypeConverter" />
    internal class IdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom([CanBeNull] ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(int))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo([CanBeNull] ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        [CanBeNull]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, [CanBeNull] object value)
        {
            if (value is string)
            {
                return new Id((string) value);
            }
            if (value is int)
            {
                return new Id((int) value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        [CanBeNull]
        public override object ConvertTo([CanBeNull] ITypeDescriptorContext context, [CanBeNull] CultureInfo culture,
            [CanBeNull] object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var id = (Id) value;
                if (id != null)
                {
                    return id.Value.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (destinationType == typeof(int))
            {
                var id = (Id) value;
                if (id != null)
                {
                    return id.Value;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
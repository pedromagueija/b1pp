// <copyright filename="IdTypeConverter.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.Types
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Standard type converter for the <see cref="Id" /> type.
    /// </summary>
    /// <seealso cref="System.ComponentModel.TypeConverter" />
    internal class IdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(int))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                return new Id(s);
            }

            if (value is int i)
            {
                return new Id(i);
            }

            return base.ConvertFrom(context, culture, value);
        }


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
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
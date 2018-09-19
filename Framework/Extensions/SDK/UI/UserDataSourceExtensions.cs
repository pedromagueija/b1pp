// <copyright filename="UserDataSourceExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.UI
{
    using System;
    using System.Globalization;

    using SAPbouiCOM;

    public static class UserDataSourcesExtensions
    {
        public static DateTime? GetDateTime(this UserDataSources datasource, string userDataSourceId)
        {
            if (datasource.IsEmpty(userDataSourceId))
            {
                return null;
            }

            var data = datasource.GetString(userDataSourceId);
            DateTime value;
            var success = DateTime.TryParse(data, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);

            if (success)
            {
                return value;
            }

            throw new InvalidOperationException(FormatMessage(userDataSourceId, data, typeof(DateTime?)));
        }

        /// <summary>
        /// Returns the double value in the datasource or null if none is found.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <returns>
        /// The double value in the datasource or null if none is found.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the value in the datasource cannot be converted to a double.
        /// </exception>
        public static double? GetDouble(this UserDataSources datasource, string userDataSourceId)
        {
            if (datasource.IsEmpty(userDataSourceId))
            {
                return null;
            }

            string data = datasource.GetString(userDataSourceId);
            double value;
            var success = double.TryParse(data, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out value);

            if (success)
            {
                return value;
            }

            throw new InvalidOperationException(FormatMessage(userDataSourceId, data, typeof(double?)));
        }

        /// <summary>
        /// Returns the double value in the datasource or null if none is found.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="defaultValue">The value to return when the datasource is empty.</param>
        /// <returns>
        /// The double value in the datasource or null if none is found.
        /// </returns>
        /// <remarks>
        /// Note that if the value in the datasource cannot be cast to a double an exception will be thrown.
        /// <para />
        /// The default value will not be returned.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the value in the datasource cannot be converted to a double.
        /// </exception>
        public static double GetDouble(
            this UserDataSources datasource,
            string userDataSourceId,
            double defaultValue)
        {
            var value = datasource.GetDouble(userDataSourceId);
            if (value != null)
            {
                return (double) value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the integer value in the datasource or null if none is found.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="defaultValue">The value to return when the datasource is empty.</param>
        /// <returns>
        /// The integer value in the datasource or null if none is found.
        /// </returns>
        /// <remarks>
        /// Note that if the value in the datasource cannot be cast to an integer an exception will be thrown.
        /// <para />
        /// The default value will not be returned.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the value in the datasource cannot be converted to an integer.
        /// </exception>
        public static int GetInt(this UserDataSources datasource, string userDataSourceId, int defaultValue)
        {
            int? value = datasource.GetInt(userDataSourceId);
            if (value != null)
            {
                return (int) value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the integer value in the datasource or null if none is found.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <returns>
        /// The integer value in the datasource or null if none is found.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the value in the datasource cannot be converted to an integer.
        /// </exception>
        public static int? GetInt(this UserDataSources datasource, string userDataSourceId)
        {
            if (datasource.IsEmpty(userDataSourceId))
            {
                return null;
            }

            string data = datasource.GetString(userDataSourceId);
            int value;
            var success = int.TryParse(data, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value);

            if (success)
            {
                return value;
            }

            throw new InvalidOperationException(FormatMessage(userDataSourceId, data, typeof(int?)));
        }

        /// <summary>
        /// Returns the string value in the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <returns>
        /// The string on the datasource.
        /// </returns>
        public static string GetString(this UserDataSources datasource, string userDataSourceId)
        {
            return datasource.Item(userDataSourceId).ValueEx ?? string.Empty;
        }

        /// <summary>
        /// Sets the string value on the user data source.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="value">The value.</param>
        /// <remarks>If value is null an empty string will be used instead.</remarks>
        /// ///
        public static void SetValue(this UserDataSources datasource, string userDataSourceId, string value)
        {
            datasource.Item(userDataSourceId).ValueEx = value ?? string.Empty;
        }

        /// <summary>
        /// Sets the integer value on the user data source.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this UserDataSources datasource, string userDataSourceId, int? value)
        {
            if (value != null)
            {
                datasource.Item(userDataSourceId).ValueEx = ((int) value).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Sets the double value on the user data source.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this UserDataSources datasource, string userDataSourceId, double? value)
        {
            if (value != null)
            {
                datasource.Item(userDataSourceId).ValueEx = ((double) value).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Sets the DateTime value on the user data source.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this UserDataSources datasource, string userDataSourceId, DateTime? value)
        {
            if (value != null)
            {
                datasource.Item(userDataSourceId).ValueEx = ((DateTime) value).ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// Formats the message to display in case of error.
        /// </summary>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Formatted message.
        /// </returns>
        private static string FormatMessage(string userDataSourceId, string data, Type type)
        {
            return $"Value '{data}' on user datasource '{userDataSourceId}' cannot be converted to a {type}.";
        }

        /// <summary>
        /// Determines whether the specified user data source is empty.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="userDataSourceId">The user data source identifier.</param>
        /// <returns>True when empty, false otherwise.</returns>
        private static bool IsEmpty(this UserDataSources datasource, string userDataSourceId)
        {
            string value = datasource.GetString(userDataSourceId);
            return string.IsNullOrEmpty(value);
        }
    }
}
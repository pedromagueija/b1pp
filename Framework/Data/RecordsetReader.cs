// <copyright filename="RecordsetReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using SAPbobsCOM;

    /// <summary>
    /// Allows you to read the data in the recordset.
    /// </summary>
    internal sealed class RecordsetReader : IRecordsetReader, IDisposable
    {
        private readonly List<Column> columns = new List<Column>();
        private readonly DisposableRecordset data;
        private IEnumerator<RecordsetRow> rows;

        /// <summary>
        /// Returns the columns.
        /// </summary>
        public IEnumerable<IColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        private RecordsetReader(DisposableRecordset data)
        {
            this.data = data;
        }

        public static RecordsetReader CreateNew(DisposableRecordset data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var reader = new RecordsetReader(data);
            reader.Load();

            return reader;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        public bool? GetBool(string columnName)
        {
            string s = GetString(columnName);
            if (s == null)
            {
                return null;
            }

            return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        public bool GetBoolOrDefault(string columnName)
        {
            return GetBool(columnName) ?? default;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        public DateTime? GetDateTime(string columnName)
        {
            string s = GetString(columnName);
            if (s == null)
            {
                return null;
            }

            return Convert.ToDateTime(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        public DateTime GetDateTimeOrDefault(string columnName)
        {
            return GetDateTime(columnName) ?? default;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        public double? GetDouble(string columnName)
        {
            string s = GetString(columnName);
            if (s == null)
            {
                return null;
            }

            return Convert.ToDouble(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        public double GetDoubleOrDefault(string columnName)
        {
            return GetDouble(columnName) ?? default;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        public int? GetInt(string columnName)
        {
            string s = GetString(columnName);
            if (s == null)
            {
                return null;
            }

            return Convert.ToInt32(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or the default value otherwise.
        /// </summary>
        public int GetIntOrDefault(string columnName)
        {
            return GetInt(columnName) ?? default;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or null otherwise.
        /// </summary>
        public string GetString(string columnName)
        {
            if (rows.Current == null)
            {
                throw new InvalidOperationException(@"Call MoveNext() first, before attempting to read any data.");
            }

            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException($@"Parameter {nameof(columnName)} cannot be null or empty.");
            }

            if (!columns.Exists(c => c.Name.Equals(columnName)))
            {
                throw new ArgumentException($@"No column with name {columnName} was found.");
            }

            var field = rows.Current.Fields.FirstOrDefault(f => f.Alias.Equals(columnName));
            return field?.Value;
        }

        /// <summary>
        /// Gets a value from the reader if one exists, or an empty string otherwise.
        /// </summary>
        public string GetStringOrEmpty(string columnName)
        {
            return GetString(columnName) ?? string.Empty;
        }

        public void Load()
        {
            var results = Deserialize();

            rows = results.Rows.GetEnumerator();
            var aliases = results.Rows.First().Fields.Select(f => f.Alias);
            var fields = data.Fields;
            foreach (string alias in aliases)
            {
                var type = fields.Item(alias).Type;

                columns.Add(new Column(alias, type));
            }
        }

        private RecordsetResult Deserialize()
        {
            string s = data.GetFixedXML(RecordsetXMLModeEnum.rxmData);
            var serializer = new XmlSerializer(typeof(RecordsetResult));
            using (var reader = XmlReader.Create(new StringReader(s)))
            {
                return (RecordsetResult) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Moves to the next record.
        /// </summary>
        /// <returns>
        /// True if there is another record, false otherwise.
        /// </returns>
        public bool MoveNext()
        {
            if (rows == null)
            {
                return false;
            }

            return rows.MoveNext();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            data?.Dispose();
            rows?.Dispose();
        }
    }
}
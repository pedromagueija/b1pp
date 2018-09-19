// <copyright filename="UserFieldExists.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Inquiries
{
    using Data;

    using SAPbobsCOM;

    /// <summary>
    /// Allows you to check the existence of user fields.
    /// </summary>
    public class UserFieldExists
    {
        private readonly Company company;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldExists" /> class.
        /// </summary>
        /// <param name="company">The company.</param>
        public UserFieldExists(Company company)
        {
            this.company = company;
        }

        /// <summary>
        /// Returns true if all field names exist on the given table.
        /// </summary>
        /// <param name="tableName">Name of the table (with the @ prefix).</param>
        /// <param name="fieldNames">The field names (without the U_ prefix).</param>
        /// <returns>
        /// Returns true if all field names exist on the given table; false otherwise.
        /// </returns>
        public bool Exists(string tableName, params string[] fieldNames)
        {
            if (string.IsNullOrWhiteSpace(tableName) || fieldNames == null || fieldNames.Length == 0)
            {
                return false;
            }

            var query = QueryFactory.Create(company);
            query.SetStatement(@"
                SELECT Count(*) AS ""Count""
                FROM ""CUFD""
                WHERE ""TableID"" = @tableName
                AND ""AliasID"" IN (@fieldNames)
            ")
                .With(@"@tableName", tableName)
                .With(@"@fieldNames", fieldNames);

            var results = query.SelectOne<dynamic>(reader => new
            {
                Count = reader.GetInt(@"Count")
            });

            return results.Count == fieldNames.Length;
        }
    }
}
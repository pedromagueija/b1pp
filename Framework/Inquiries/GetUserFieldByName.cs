// <copyright filename="GetUserFieldByName.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Inquiries
{
    using Data;

    using SAPbobsCOM;

    /// <summary>
    /// Allows you to get a user field using its tablename and fieldname.
    /// </summary>
    public class GetUserFieldByName
    {
        private readonly Company company;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserFieldByName"/> class.
        /// </summary>
        /// <param name="company">The company.</param>
        public GetUserFieldByName(Company company)
        {
            this.company = company;
        }

        /// <summary>
        /// Finds a specific field using its table and name.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>
        /// The <see cref="UserFieldsMD" /> object.
        /// If the user field doesn't exist or can't be loaded <see langword="null" /> is returned.
        /// </returns>
        public UserFieldsMD Find(string tableName, string fieldName)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var query = QueryFactory.Create(company);
            query.SetStatement(@"
                SELECT ""FieldID""
                FROM ""CUFD""
                WHERE ""TableID"" = @tableName
                AND ""AliasID"" = @fieldName
            ")
                .With(@"@tableName", tableName)
                .With(@"@fieldName", fieldName);

            var result = query.SelectOne();
            if (result == null)
            {
                return null;
            }

            var userField = company.GetBusinessObject(BoObjectTypes.oUserFields) as UserFieldsMD;
            bool success = userField?.GetByKey(tableName, result.FieldID) ?? false;

            if (!success)
            {
                return null;
            }

            return userField;
        }
    }
}
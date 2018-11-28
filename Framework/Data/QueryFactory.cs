// <copyright filename="QueryFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using SAPbobsCOM;

    /// <summary>
    /// Allows queries to be created.
    /// </summary>
    public static class QueryFactory
    {
        /// <summary>
        /// Creates a new query.
        /// </summary>
        /// <param name="company">
        /// The company this query will be run against.
        /// </param>
        /// <returns>
        /// A new query object that you can use to run your query.
        /// </returns>
        public static IQuery Create(Company company)
        {
            return new Query(company);
        }

        public static IQuery Create(Company company, string sql)
        {
            var query = new Query(company);
            query.SetStatement(sql);

            return query;
        }
    }
}
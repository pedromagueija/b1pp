// <copyright filename="QueryFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using SAPbobsCOM;

    public static class QueryFactory
    {
        public static IQuery Create(Company company)
        {
            return new Query(company);
        }
    }
}
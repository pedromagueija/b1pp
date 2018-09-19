// <copyright filename="SqlStatementFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    public static class SqlStatementFactory
    {
        public static ISelectStatement<T> Create<T>()
        {
            return new SelectStatement<T>();
        }
    }
}
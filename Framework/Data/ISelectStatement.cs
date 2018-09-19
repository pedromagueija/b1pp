// <copyright filename="ISelectStatement.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Linq.Expressions;

    public interface ISelectStatement<T>
    {
        string GetStatement();
        void Where(Expression<Func<T, bool>> predicate);
    }
}
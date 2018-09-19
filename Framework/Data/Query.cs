// <copyright filename="Query.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;

    using SAPbobsCOM;

    /// <summary>
    /// Facilitates the querying operations by acting as a simple facade.
    /// <para />
    /// It also allows a fluent usage.
    /// <example>
    /// var q = new Query(company);
    /// q.SetStatement(sql).With("parameter", "value");
    /// </example>
    /// </summary>
    /// <seealso cref="QueryBase" />
    internal class Query : QueryBase, IQuery
    {
        private readonly List<IQueryArg> args = new List<IQueryArg>();

        /// <summary>
        /// Gets or sets the query statement.
        /// </summary>
        /// <value>
        /// The statement without any processing.
        /// </value>
        public string Statement { get; set; }

        /// <summary>
        /// Gets the prepared statement.
        /// </summary>
        /// <value>
        /// The prepared statement with all parameters set to their corresponding values.
        /// </value>
        public string PreparedStatement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Statement))
                {
                    return string.Empty;
                }

                return Prepare(Statement, args.ToArray());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query" /> class.
        /// </summary>
        /// <param name="company">The company.</param>
        public Query(Company company) : base(company)
        {
        }

        /// <summary>
        /// Adds an argument to the argument list.
        /// </summary>
        /// <param name="arg">The argument to add.</param>
        public void AddArgument(IQueryArg arg)
        {
            args.Add(arg);
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectMany()
        {
            return SelectMany(PreparedStatement);
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> SelectMany<T>() where T : class
        {
            return SelectMany<T>(PreparedStatement);
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public IEnumerable<T> SelectMany<T>(InstanceCreator<T> creator) where T : class
        {
            return SelectMany(PreparedStatement, creator);
        }

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SelectOne<T>() where T : class
        {
            return SelectOne<T>(PreparedStatement);
        }

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <returns></returns>
        public dynamic SelectOne()
        {
            return SelectOne(PreparedStatement);
        }

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public T SelectOne<T>(InstanceCreator<T> creator) where T : class
        {
            return SelectOne(PreparedStatement, creator, null);
        }

        /// <summary>
        /// Sets the query statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns>
        /// The current object so you can continue chaining calls.
        /// </returns>
        public IQuery SetStatement(string statement)
        {
            Statement = statement;
            return this;
        }

        /// <summary>
        /// Sets the query statement automatically to the provided type.
        /// </summary>
        /// <returns>
        /// The current object so you can continue chaining calls.
        /// </returns>
        public IQuery SetStatement<T>()
        {
            var statement = SqlStatementFactory.Create<T>();
            Statement = statement.GetStatement();
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, int value)
        {
            args.Add(new IntegerValueArg(placeholder, value));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, string value)
        {
            args.Add(new StringValueArg(placeholder, value));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, DateTime value)
        {
            args.Add(new DateTimeValueArg(placeholder, value));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, double value)
        {
            args.Add(new DoubleValueArg(placeholder, value));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, IEnumerable<int> values)
        {
            args.Add(new MultipleIntValuesArg(placeholder, values));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, IEnumerable<double> values)
        {
            args.Add(new MultipleDoubleValuesArg(placeholder, values));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, IEnumerable<string> values)
        {
            args.Add(new MultipleStringValuesArg(placeholder, values));
            return this;
        }

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IQuery With(string placeholder, IEnumerable<DateTime> values)
        {
            args.Add(new MultipleDateTimeValuesArg(placeholder, values));
            return this;
        }

        /// <summary>
        /// Executes the current query.
        /// </summary>
        public void Execute()
        {
            Execute(PreparedStatement);
        }
    }
}
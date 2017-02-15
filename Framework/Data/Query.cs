// <copyright filename="Query.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using SAPbobsCOM;

    /// <summary>
    /// Facilitates the querying operations by acting as a simple facade.<para/>
    /// It also allows a fluent usage.
    /// <example>
    /// var q = new Query(company);
    /// q.SetStatement(sql).With("parameter", "value);
    /// </example>
    /// </summary>
    /// <seealso cref="B1PP.Data.QueryHelper" />
    public class Query : QueryHelper
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
                if(string.IsNullOrWhiteSpace(Statement))
                    return string.Empty;

                return Prepare(Statement, args.ToArray());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
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
        /// Sets the query statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns>
        /// The current object so you can continue chaining calls.
        /// </returns>
        public Query SetStatement(string statement)
        {
            Statement = statement;
            return this;
        }

        public IEnumerable<dynamic> SelectMany()
        {
            return SelectMany(PreparedStatement);
        }

        public IEnumerable<T> SelectMany<T>() where T : class
        {
            return SelectMany<T>(PreparedStatement);
        }

        public IEnumerable<T> SelectMany<T>(InstanceCreator<T> creator) where T : class
        {
            return SelectMany(PreparedStatement, creator);
        }

        [CanBeNull]
        public T SelectOne<T>() where T : class
        {
            return SelectOne<T>(PreparedStatement);
        }

        [CanBeNull]
        public dynamic SelectOne()
        {
            return SelectOne(PreparedStatement);
        }

        [CanBeNull]
        public T SelectOne<T>(InstanceCreator<T> creator) where T : class
        {
            return SelectOne(PreparedStatement, creator, null);
        }

        public Query With(string placeholder, int value)
        {
            args.Add(new IntegerValueArg(placeholder, value));
            return this;
        }

        public Query With(string placeholder, string value)
        {
            args.Add(new StringValueArg(placeholder, value));
            return this;
        }

        public Query With(string placeholder, DateTime value)
        {
            args.Add(new DateTimeValueArg(placeholder, value));
            return this;
        }

        public Query With(string placeholder, double value)
        {
            args.Add(new DoubleValueArg(placeholder, value));
            return this;
        }

        public Query With(string placeholder, IEnumerable<int> values)
        {
            args.Add(new MultipleIntValuesArg(placeholder, values));
            return this;
        }

        public Query With(string placeholder, IEnumerable<double> values)
        {
            args.Add(new MultipleDoubleValuesArg(placeholder, values));
            return this;
        }

        public Query With(string placeholder, IEnumerable<string> values)
        {
            args.Add(new MultipleStringValuesArg(placeholder, values));
            return this;
        }

        public Query With(string placeholder, IEnumerable<DateTime> values)
        {
            args.Add(new MultipleDateTimeValuesArg(placeholder, values));
            return this;
        }
    }
}
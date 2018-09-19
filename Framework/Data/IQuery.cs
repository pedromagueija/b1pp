namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows you to execute queries and access results.
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Adds an argument to the argument list.
        /// </summary>
        /// <param name="arg">The argument to add.</param>
        void AddArgument(IQueryArg arg);

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> SelectMany();

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> SelectMany<T>() where T : class;

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        IEnumerable<T> SelectMany<T>(InstanceCreator<T> creator) where T : class;

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T SelectOne<T>() where T : class;

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <returns></returns>
        dynamic SelectOne();

        /// <summary>
        /// Selects the one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        T SelectOne<T>(InstanceCreator<T> creator) where T : class;

        /// <summary>
        /// Sets the query statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns>
        /// The current object so you can continue chaining calls.
        /// </returns>
        IQuery SetStatement(string statement);

        /// <summary>
        /// Sets the query statement automatically to the provided type.
        /// </summary>
        /// <returns>
        /// The current object so you can continue chaining calls.
        /// </returns>
        IQuery SetStatement<T>();

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IQuery With(string placeholder, int value);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IQuery With(string placeholder, string value);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IQuery With(string placeholder, DateTime value);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IQuery With(string placeholder, double value);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IQuery With(string placeholder, IEnumerable<int> values);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IQuery With(string placeholder, IEnumerable<double> values);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IQuery With(string placeholder, IEnumerable<string> values);

        /// <summary>
        /// Adds the specified pair as an argument for the query.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IQuery With(string placeholder, IEnumerable<DateTime> values);

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        IEnumerable<T> SelectMany<T>(string query, InstanceCreator<T> creator);

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<object> SelectMany(string query);

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<T> SelectMany<T>(string query) where T : class;

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        T SelectOne<T>(string query, InstanceCreator<T> creator, T @default) where T : class;

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        T SelectOne<T>(string query) where T : class;

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        dynamic SelectOne(string query);

        /// <summary>
        /// Executes the current query.
        /// </summary>
        void Execute();
    }
}
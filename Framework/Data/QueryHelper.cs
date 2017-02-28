// <copyright filename="QueryHelper.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using SAPbobsCOM;

    /// <summary>
    /// Performs data access operations.
    /// </summary>
    public class QueryHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public delegate T InstanceCreator<out T>(IRecordsetReader reader);

        /// <summary>
        /// The company
        /// </summary>
        private readonly Company company;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHelper"/> class.
        /// </summary>
        /// <param name="company">The company.</param>
        public QueryHelper(Company company)
        {
            this.company = company;
        }

        /// <summary>
        /// Prepares the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public string Prepare(string query, params IQueryArg[] args)
        {
            string workingQuery = query;

            foreach (IQueryArg arg in args)
            {
                workingQuery = workingQuery.Replace(arg.PlaceHolder, arg.Value);
            }

            return workingQuery;
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public IEnumerable<T> SelectMany<T>(string query, InstanceCreator<T> creator)
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);

                if (recordset.RecordCount <= 0)
                {
                    yield break;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                while (reader.MoveNext())
                {
                    yield return creator(reader);
                }
            }
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectMany(string query)
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);

                if (recordset.RecordCount <= 0)
                {
                    yield break;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                while (reader.MoveNext())
                {
                    yield return InstanceFactory.CreateDynamicObject(reader);
                }
            }
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IEnumerable<T> SelectMany<T>(string query) where T : class
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);

                if (recordset.RecordCount <= 0)
                {
                    yield break;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                while (reader.MoveNext())
                {
                    yield return InstanceFactory.AutoCreateInstance<T>(reader);
                }
            }
        }

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        public T SelectOne<T>(string query, InstanceCreator<T> creator, T @default) where T : class
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);

                if (recordset.RecordCount <= 0)
                {
                    return @default;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                if (reader.MoveNext())
                {
                    return creator(reader);
                }

                return @default;
            }
        }

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [CanBeNull]
        public T SelectOne<T>(string query) where T : class
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);

                if (recordset.RecordCount <= 0)
                {
                    return null;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                if (reader.MoveNext())
                {
                    return InstanceFactory.AutoCreateInstance<T>(reader);
                }

                return null;
            }
        }

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [CanBeNull]
        public dynamic SelectOne(string query)
        {
            using (var recordset = new DisposableRecordset(company))
            {
                recordset.DoQuery(query);
                if (recordset.RecordCount <= 0)
                {
                    return null;
                }

                XmlRecordsetReader reader = XmlRecordsetReader.CreateNew(recordset);

                if (reader.MoveNext())
                {
                    return InstanceFactory.CreateDynamicObject(reader);
                }

                return null;
            }
        }
    }
}
// <copyright filename="QueryHelper.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using SAPbobsCOM;

    public class QueryHelper
    {
        public delegate T InstanceCreator<out T>(IRecordsetReader reader);

        private readonly Company company;

        public QueryHelper(Company company)
        {
            this.company = company;
        }

        public string Prepare(string query, params IQueryArg[] args)
        {
            string workingQuery = query;

            foreach (IQueryArg arg in args)
            {
                workingQuery = workingQuery.Replace(arg.PlaceHolder, arg.Value);
            }

            return workingQuery;
        }

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
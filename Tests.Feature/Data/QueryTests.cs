// <copyright filename="QueryTests.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Data
{
    using System;
    using System.Linq;

    using B1PP.Connections;
    using B1PP.Data;

    using NUnit.Framework;

    internal class QueryTests
    {
        private IStandardConnection connection;

        [Test]
        public void CanGetDynamicObject()
        {
            var query = new Query(connection.Company);
            query.SetStatement(@"
            SELECT TOP 1
                'string value' as ""String"",
                1 AS ""Number"",
                12.3 AS ""Float"",
                CAST('20120101' AS DateTime) AS ""DateTime""
            FROM OCRD");

            var results = query.SelectOne();

            Assert.NotNull(results);

            Assert.AreEqual("string value", results.String);
            Assert.AreEqual(typeof(string), results.String.GetType());

            Assert.AreEqual(1, results.Number);
            Assert.AreEqual(typeof(int), results.Number.GetType());

            Assert.AreEqual(12.3, results.Float);
            Assert.AreEqual(typeof(double), results.Float.GetType());

            Assert.AreEqual(new DateTime(2012, 1, 1), results.DateTime);
            Assert.AreEqual(typeof(DateTime), results.DateTime.GetType());
        }

        [Test]
        public void CanGetListDynamicObject()
        {
            var query = new Query(connection.Company);
            query.SetStatement(@"
            SELECT TOP 10
                'string value' as ""String"",
                1 AS ""Number"",
                12.3 AS ""Float"",
                CAST('20120101' AS DateTime) AS ""DateTime""
            FROM OCRD");

            var many = query.SelectMany().ToList();

            Assert.NotNull(many);
            Assert.AreEqual(10, many.Count);

            var first = many.First();

            Assert.AreEqual("string value", first.String);
            Assert.AreEqual(typeof(string), first.String.GetType());

            Assert.AreEqual(1, first.Number);
            Assert.AreEqual(typeof(int), first.Number.GetType());

            Assert.AreEqual(12.3, first.Float);
            Assert.AreEqual(typeof(double), first.Float.GetType());

            Assert.AreEqual(new DateTime(2012, 1, 1), first.DateTime);
            Assert.AreEqual(typeof(DateTime), first.DateTime.GetType());
        }

        [SetUp]
        public void Setup()
        {
            connection = ConnectionFactory.CreateStandardConnection();
            connection.Connect();
        }

        [TearDown]
        public void Teardown()
        {
            connection.Disconnect();
        }
    }
}
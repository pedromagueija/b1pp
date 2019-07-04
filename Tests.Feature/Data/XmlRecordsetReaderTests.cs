// <copyright filename="XmlRecordsetReaderTests.cs" project="Tests.Feature">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Data
{
    using System;
    using B1PP.Connections;
    using B1PP.Data;
    using NUnit.Framework;
    using SAPbobsCOM;

    internal class XmlRecordsetReaderTests
    {
        [Test]
        public void GetInt_Fails_On_Overflow()
        {
            var b1 = ConnectionFactory.CreateStandardConnection();
            b1.Connect();

            using (var rs = new DisposableRecordset(b1.Company))
            {
                rs.DoQuery(@"SELECT TOP 1 2147483647+1 AS ""Number"" FROM ""OCRD"" ");
                var reader = RecordsetReader.CreateNew(rs);
                reader.MoveNext();

                Assert.Throws<OverflowException>(() => reader.GetInt(@"Number"));
            }

            b1.Disconnect();
        }

        [Test]
        public void METHOD()
        {
            var b1 = ConnectionFactory.CreateStandardConnection();
            b1.Connect();
            using (var rs = new DisposableRecordset(b1.Company))
            {
                rs.DoQuery(@"SELECT * FROM ""OCRD"" ");
                var reader = RecordsetReader.CreateNew(rs);

                while (reader.MoveNext())
                {
                    string name = reader.GetString(@"CardName");
                    string code = reader.GetString(@"CardCode");
                    int group = reader.GetInt(@"GroupCode").GetValueOrDefault();

                    Console.WriteLine($@"{name} {code} {group}");
                }
            }

            b1.Disconnect();
        }
    }
}
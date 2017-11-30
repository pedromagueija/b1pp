// <copyright filename="CanCreateSimpleTableTests.cs" project="B1PP.DI.Schema.Tests.Features">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Feature.Database
{
    using System;
    using System.Collections.Generic;

    using B1PP.Connections;
    using B1PP.Database;
    using B1PP.Database.Attributes;

    using NUnit.Framework;

    using SAPbobsCOM;

    internal class AttributeMetadataTests
    {
        private Company company;

        [SetUp]
        public void Setup()
        {
            var connection = ConnectionFactory.CreateStandardConnection();
            connection.Connect();

            company = connection.Company;

            var manager = new SchemaManager(company);
            manager.InitializeFromAssembly(GetType().Assembly);
        }

        [Test]
        public void CanCreateSimpleTable()
        {
            Assert.NotNull(company.UserTables.Item(@"T_SPTB"));
        }

        [Test]
        public void CanCreateFields()
        {
            var userTable = company.UserTables.Item(@"T_SPTB");
            Assert.NotNull(userTable);

            var userFields = userTable.UserFields.Fields;
            Assert.NotNull(userFields.Item(@"U_Subject"));
            Assert.NotNull(userFields.Item(@"U_NumberOfDays"));
            Assert.NotNull(userFields.Item(@"U_Price"));
            Assert.NotNull(userFields.Item(@"U_Date"));
            Assert.NotNull(userFields.Item(@"U_Type"));
        }


        [Test]
        public void CanCreateDocumentTable()
        {
            AssertTable(@"T_DOTB", @"Document object table", BoUTBTableType.bott_Document);

            var userTable = company.UserTables.Item(@"T_DOTB");
            var userFields = userTable.UserFields.Fields;
            Assert.NotNull(userFields.Item(@"U_ReferenceNumber"));
            Assert.NotNull(userFields.Item(@"U_CustomerId"));
        }

        [Test]
        public void CanCreateDocumentTableObject()
        {
            AssertObject(@"T_DOTB_O", BoUTBTableType.bott_Document);
        }

        private void AssertObject(string objectId, BoUTBTableType objectType)
        {
            var ubmd = (UserObjectsMD)company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            Assert.IsTrue(ubmd.GetByKey(objectId));
        }

        private void AssertTable(string tableName, string tableDescription, BoUTBTableType tableType)
        {
            var utmd = (UserTablesMD) company.GetBusinessObject(BoObjectTypes.oUserTables);
            Assert.IsTrue(utmd.GetByKey(tableName));
            Assert.AreEqual(utmd.TableType, tableType);
            Assert.IsTrue(string.Equals(utmd.TableDescription, tableDescription, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(utmd.TableName, tableName, StringComparison.OrdinalIgnoreCase));
        }


        [Test]
        public void CanCreateAutoTableName()
        {
            AssertTable(@"T_AutoTableName", @"Auto Table Name", BoUTBTableType.bott_NoObject);
        }
    }

    [UserTable(BoUTBTableType.bott_NoObject, @"T_")]
    internal class AutoTableName : SimpleRecord
    {
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10)]
        public string SimpleField { get; set; }

        public string IgnoredField { get; set; }
    }

    [UserTable(@"SPTB", @"Test no object table", BoUTBTableType.bott_NoObject, @"T_")]
    internal class Sample : SimpleRecord
    {
        [FieldName(nameof(Subject), @"Subject of Sample")]
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        public string Subject { get; set; }

        [UserField(BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2)]
        public int NumberOfDays { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_Price)]
        public double Price { get; set; }

        [UserField(BoFieldTypes.db_Date, BoFldSubTypes.st_None)]
        public DateTime Date { get; set; }

        [UserFieldValidValue(@"0", @"Regular")]
        [UserFieldValidValue(@"1", @"Advanced")]
        [UserFieldValidValue(@"2", @"Unique")]
        [UserField(BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1)]
        public SampleType Type { get; set; }
    }

    internal enum SampleType
    {
        Regular,
        Advanced,
        Unique
    }

    [UserTable(@"DOTB", @"Document object table", BoUTBTableType.bott_Document, @"T_")]
    [UserObject(@"T_DOTB_O", @"Document object table object", BoUDOObjType.boud_Document)]
    [UserObjectServices(ObjectServices.Find, ObjectServices.Delete, ObjectServices.Cancel, ObjectServices.Close, ObjectServices.Log, ObjectServices.ManageSeries, ObjectServices.YearTransfer)]
    [ApproveService(@"MyTmplId")]
    internal class DocumentSample : DocumentRecord
    {
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        public string CustomerId { get; set; }

        [FieldName(nameof(ReferenceNumber), @"Customer Reference Number")]
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        [UserFieldOptional]
        public string ReferenceNumber { get; set; }

        [ChildUserTable(@"T_DTLN", @"T_DTLN_O", @"T_DTLN_LOG")]
        public IEnumerable<DocumentSampleLine> Lines { get; } = new List<DocumentSampleLine>();
    }

    [UserTable(@"DTLN", @"Document object table lines", BoUTBTableType.bott_DocumentLines, @"T_")]
    internal class DocumentSampleLine : DocumentRecordLine
    {
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50)]
        public string ItemCode { get; set; }

        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        public string ItemDescription { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity)]
        public double Quantity { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_Price)]
        public double UnitPrice { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage)]
        public double DiscountPercentage { get; set; }
    }
}
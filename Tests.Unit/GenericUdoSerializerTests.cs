namespace Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B1PP;
    using B1PP.Database.Attributes;

    using NUnit.Framework;

    using SAPbobsCOM;

    public class GenericUdoSerializerTests
    {
        [Test]
        public void Can_Deserialize_Master_And_Detail_Data()
        {
            var serializer = new GenericUdoSerializer<TestUdo>();
            var instance = serializer.Deserialize(TestUdoConstants.Out);

            AssertMatch(CreateTestUdo(), instance);
        }

        [Test]
        public void Can_Serialize_Master_And_Detail_Data()
        {
            var instance = CreateTestUdo();

            var serializer = new GenericUdoSerializer<TestUdo>();

            string xml = serializer.Serialize(instance);

            Assert.AreEqual(TestUdoConstants.Out, xml);
        }

        [Test]
        public void Can_Deserialize_When_Value_Is_Missing_In_Xml()
        {
            var serializer = new GenericUdoSerializer<TestUdo>();
            var instance = serializer.Deserialize(TestUdoConstants.Out2);
            
            Assert.AreEqual(default(string), instance.AlphaNumericalField);
            Assert.AreEqual(default(int), instance.IntegerField);
            Assert.AreEqual(default(double), instance.DoubleField);
            Assert.AreEqual(default(DateTime), instance.DateTimeField);
        }

        private void AssertMatch(TestUdo expected, TestUdo actual)
        {
            Assert.AreEqual(expected.AlphaNumericalField, actual.AlphaNumericalField);
            Assert.AreEqual(expected.IntegerField, actual.IntegerField);
            Assert.AreEqual(expected.DoubleField, actual.DoubleField);
            Assert.AreEqual(expected.DateTimeField, actual.DateTimeField);
            Assert.AreEqual(expected.MemoField, actual.MemoField);

            for (int i = 0; i < expected.CollectionField.Count; i++)
            {
                var value = expected.CollectionField.ElementAt(i);
                var other = actual.CollectionField.ElementAt(i);

                Assert.AreEqual(value.ChildAlphaNumericalField, other.ChildAlphaNumericalField);
                Assert.AreEqual(value.ChildIntegerField, other.ChildIntegerField);
                Assert.AreEqual(value.ChildDoubleField, other.ChildDoubleField);
                Assert.AreEqual(value.ChildDateTimeField, other.ChildDateTimeField);
                Assert.AreEqual(value.ChildMemoField, other.ChildMemoField);
            }
        }

        private static TestUdo CreateTestUdo()
        {
            // master
            var instance = new TestUdo
            {
                AlphaNumericalField = @"Alpha",
                IntegerField = 1234,
                DoubleField = 12.34,
                DateTimeField = new DateTime(2001, 1, 1),
                MemoField = @"Memo"
            };

            // detail
            var collection = new TestUdoChild
            {
                ChildAlphaNumericalField = @"Alpha",
                ChildIntegerField = 1234,
                ChildDoubleField = 12.34,
                ChildDateTimeField = new DateTime(2001, 1, 1),
                ChildMemoField = @"Memo"
            };

            instance.CollectionField.Add(collection);
            return instance;
        }
    }

    [UserTable(@"MyTableName", @"My Table Description", BoUTBTableType.bott_MasterData)]
    [UserObject(@"MyId", @"My Name", BoUDOObjType.boud_MasterData)]
    internal class TestUdo
    {
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        public string AlphaNumericalField { get; set; }

        [UserField(BoFieldTypes.db_Numeric, BoFldSubTypes.st_None)]
        public int IntegerField { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_None)]
        public double DoubleField { get; set; }

        [UserField(BoFieldTypes.db_Date, BoFldSubTypes.st_None)]
        public DateTime DateTimeField { get; set; }

        [UserField(BoFieldTypes.db_Memo, BoFldSubTypes.st_None)]
        public string MemoField { get; set; }

        [Children(typeof(TestUdoChild))]
        public ICollection<TestUdoChild> CollectionField { get; } = new List<TestUdoChild>();
    }

    [ChildUserTable(@"MyChildTableName", @"Child Table Description", BoUTBTableType.bott_MasterDataLines)]
    internal class TestUdoChild
    {
        [UserField(BoFieldTypes.db_Alpha, BoFldSubTypes.st_None)]
        public string ChildAlphaNumericalField { get; set; }

        [UserField(BoFieldTypes.db_Numeric, BoFldSubTypes.st_None)]
        public int ChildIntegerField { get; set; }

        [UserField(BoFieldTypes.db_Float, BoFldSubTypes.st_None)]
        public double ChildDoubleField { get; set; }

        [UserField(BoFieldTypes.db_Date, BoFldSubTypes.st_None)]
        public DateTime ChildDateTimeField { get; set; }

        [UserField(BoFieldTypes.db_Memo, BoFldSubTypes.st_None)]
        public string ChildMemoField { get; set; }
    }

    internal static class TestUdoConstants
    {
        public const string Out = @"<My_Name><U_AlphaNumericalField>Alpha</U_AlphaNumericalField><U_IntegerField>1234</U_IntegerField><U_DoubleField>12.34</U_DoubleField><U_DateTimeField>2001-01-01</U_DateTimeField><U_MemoField>Memo</U_MemoField><MyChildTableName_OCollection><MyChildTableName_O><U_ChildAlphaNumericalField>Alpha</U_ChildAlphaNumericalField><U_ChildIntegerField>1234</U_ChildIntegerField><U_ChildDoubleField>12.34</U_ChildDoubleField><U_ChildDateTimeField>2001-01-01</U_ChildDateTimeField><U_ChildMemoField>Memo</U_ChildMemoField></MyChildTableName_O></MyChildTableName_OCollection></My_Name>";
        public const string Out2 = @"<My_Name><U_AlphaNumericalField></U_AlphaNumericalField><U_IntegerField></U_IntegerField><U_DoubleField></U_DoubleField><U_DateTimeField></U_DateTimeField><U_MemoField></U_MemoField><MyChildTableName_OCollection><MyChildTableName_O><U_ChildAlphaNumericalField>Alpha</U_ChildAlphaNumericalField><U_ChildIntegerField>1234</U_ChildIntegerField><U_ChildDoubleField>12.34</U_ChildDoubleField><U_ChildDateTimeField>2001-01-01</U_ChildDateTimeField><U_ChildMemoField>Memo</U_ChildMemoField></MyChildTableName_O></MyChildTableName_OCollection></My_Name>";
        
    }
}
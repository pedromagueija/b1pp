// <copyright filename="UserObjectAdapterTests.cs" project="B1PP.DI.Schema.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Database
{
    using System;
    using System.Xml.Linq;

    using B1PP.Database;

    using NSubstitute;

    using NUnit.Framework;

    using SAPbobsCOM;

    internal class UserObjectAdapterTests
    {
        [Test]
        public void CanCreateAdapter()
        {
            var userObject = Substitute.For<UserObjectsMD>();
            var xmlUserObject = new XElement("UserObject");

            Assert.NotNull(new UserObjectAdapter(userObject, xmlUserObject));
        }


        [Test]
        public void CannotCreateAdapterWithoutUserObjectMd()
        {
            var xmlUserObject = new XElement("UserObject");

            Assert.Throws<ArgumentNullException>(() => { new UserObjectAdapter(null, xmlUserObject); });
        }

        [Test]
        public void CannotCreateAdapterWithoutXmlUserObject()
        {
            var userObject = Substitute.For<UserObjectsMD>();

            Assert.Throws<ArgumentNullException>(() => { new UserObjectAdapter(userObject, null); });
        }


        [Test]
        public void ConvertsXmlRepresentationIntoObject()
        {
            var userObject = Substitute.For<IUserObjectsMD>();
            var xmlUserObject= XElement.Parse(@"

                 <UserObject TableName=""TableName"" CanLog=""tYES"">
                  <FindColumns>
                    <FindColumn ColumnAlias=""ColAlias"" ColumnDescription=""ColDesc"" />
                  </FindColumns>
                  <ChildTables>
                    <ChildTable TableName=""ChildTableName"" />
                  </ChildTables>
                </UserObject>
            ");

            var adapter = new UserObjectAdapter(userObject, xmlUserObject);
            adapter.Execute();

            Assert.AreEqual("TableName", userObject.TableName);
            Assert.AreEqual(BoYesNoEnum.tYES, userObject.CanLog);
            Assert.AreEqual("ColAlias", userObject.FindColumns.ColumnAlias);
            Assert.AreEqual("ChildTableName", userObject.ChildTables.TableName);
        }


    }
}
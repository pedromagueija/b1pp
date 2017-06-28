// <copyright filename="DisposableRecordsetTests.cs" project="Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Data
{
    using B1PP.Data;

    using NSubstitute;

    using NUnit.Framework;

    using SAPbobsCOM;

    internal class DisposableRecordsetTests
    {

        [Test]
        public void DisposesRecordsetAfterUse()
        {
            var mockRs = Substitute.For<Recordset>();
            DisposableRecordset rs;
            using (rs = new DisposableRecordset(mockRs))
            {
                Assert.IsFalse(rs.IsDisposed);
            }

            Assert.IsTrue(rs.IsDisposed);
        }


    }
}
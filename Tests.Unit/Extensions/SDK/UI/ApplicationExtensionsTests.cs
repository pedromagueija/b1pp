// <copyright filename="ApplicationExtensionsTests.cs" project="B1PP.Extensions.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.Tests.Unit.SDK.UI
{
    using System.Runtime.InteropServices;

    using Extensions.SDK.UI;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    using SAPbouiCOM;

    public class ApplicationExtensionsTests
    {

        [Test]
        public void GetActiveFormId_ReturnsEmptyStringIfActiveFormThrowsException()
        {
            var app = Substitute.For<Application>();
            app.Forms.ActiveForm.Returns(t => { throw new COMException(); });

            var formId = app.GetActiveFormId();

            Assert.AreEqual(string.Empty, formId);
        }

        [Test]
        public void GetActiveFormId_ReturnsFormId()
        {
            var app = Substitute.For<Application>();
            var form = Substitute.For<Form>();
            form.UniqueID.Returns(@"FORM_UID");
            app.Forms.ActiveForm.Returns(form);

            var formId = app.GetActiveFormId();

            Assert.AreEqual(@"FORM_UID", formId);
        }

        [Test]
        public void GetActiveFormId_ReturnsEmptyStringIfFormIdIsNull()
        {
            var app = Substitute.For<Application>();
            var form = Substitute.For<Form>();
            form.UniqueID.Returns((string)null);
            app.Forms.ActiveForm.Returns(form);

            var formId = app.GetActiveFormId();

            Assert.AreEqual(string.Empty, formId);
        }
    }
}
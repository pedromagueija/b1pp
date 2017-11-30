// <copyright filename="DiApiConnectionSettingsTests.cs" project="B1PP.Connections.Tests.Unit">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace Tests.Unit.Connections
{
    using System.IO;
    using System.Reflection;

    using B1PP.Connections;

    using NUnit.Framework;

    using SAPbobsCOM;

    [TestFixture]
    public class DiApiConnectionSettingsTests
    {
        private void AssertSettingsMatch(DiApiConnectionSettings settings, DiApiConnectionSettings expectedSettings)
        {
            Assert.AreEqual(expectedSettings.Server, settings.Server);
            Assert.AreEqual(expectedSettings.UserName, settings.UserName);
            Assert.AreEqual(expectedSettings.Language, settings.Language);
            Assert.AreEqual(expectedSettings.LicenseServer, settings.LicenseServer);
            Assert.AreEqual(expectedSettings.DbUserName, settings.DbUserName);
            Assert.AreEqual(expectedSettings.UseTrusted, settings.UseTrusted);
        }

        private string CreateConfigFile()
        {
            var path = Path.GetTempFileName();
            var resourceName = $@"{GetType().Namespace}.config.template.xml";

            File.WriteAllText(path, ReadText(resourceName));

            return path;
        }

        private string ReadText(string resourceName)
        {
            var assembly = Assembly.GetAssembly(this.GetType());

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [Test]
        public void LoadSettingsFromConfigurationTest()
        {
            var expectedSettings = DiApiConnectionSettings.CreateTrustedSettings(
                "server",
                "user",
                "password",
                BoSuppLangs.ln_English,
                "server:30000",
                BoDataServerTypes.dst_MSSQL2012,
                "SBODEMOCH");

            var configFile = CreateConfigFile();

            var settings = DiApiConnectionSettings.Load(configFile);

            AssertSettingsMatch(settings, expectedSettings);
        }

        [Test]
        [Ignore("Mono fails with cannot find ole32.dll.")]
        public void ToCompanyReturnsCompanyWithSettings()
        {
            var expectedSettings = DiApiConnectionSettings.CreateStandardSettings(
                "server",
                userName: "user",
                password: "password",
                language: BoSuppLangs.ln_English,
                licenseServer: "server:30000",
                dbUserName: "dbUser",
                dbPassword: "dbPass",
                dbServerType: BoDataServerTypes.dst_MSSQL2012,
                companyDb: "SBODEMOCH");

            var company = expectedSettings.ToCompany();

            // can't check passwords
            Assert.That(company.Server, Is.EqualTo(expectedSettings.Server));
            Assert.That(company.UserName, Is.EqualTo(expectedSettings.UserName));
            Assert.That(company.language, Is.EqualTo(expectedSettings.Language));
            Assert.That(company.LicenseServer, Is.EqualTo(expectedSettings.LicenseServer));
            Assert.That(company.DbUserName, Is.EqualTo(expectedSettings.DbUserName));
            Assert.That(company.UseTrusted, Is.EqualTo(expectedSettings.UseTrusted));
        }
    }
}
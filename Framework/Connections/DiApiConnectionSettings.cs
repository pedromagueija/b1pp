// <copyright filename="DiApiConnectionSettings.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Connections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    using SAPbobsCOM;

    /// <summary>
    /// These settings will be used when establishing the connection to the DI API.
    /// </summary>
    public class DiApiConnectionSettings
    {
        /// <summary>
        /// The company database (e.g.: SBODEMOUS).
        /// </summary>
        private string companyDb;

        /// <summary>
        /// The database password.
        /// </summary>
        private string dbPassword;

        /// <summary>
        /// The database user name.
        /// </summary>
        private string dbUserName;

        /// <summary>
        /// The license server.
        /// </summary>
        private string licenseServer;

        /// <summary>
        /// The company password.
        /// </summary>
        private string password;

        /// <summary>
        /// The server.
        /// </summary>
        private string server;

        /// <summary>
        /// The company user name.
        /// </summary>
        private string userName;

        /// <summary>
        /// Gets or sets the company database name.
        /// </summary>
        /// <value>
        /// Company database name (e.g.: SBODEMOUS).
        /// </value>
        public string CompanyDb
        {
            get
            {
                return companyDb;
            }
            set
            {
                companyDb = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the database password. Relevant only when UseTrusted is false.
        /// </summary>
        /// <value>
        /// Password of the database server user.
        /// </value>
        public string DbPassword
        {
            get
            {
                return UseTrusted ? string.Empty : dbPassword;
            }
            set
            {
                dbPassword = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the type of the database server.
        /// </summary>
        /// <value>
        /// Database server type.
        /// </value>
        public BoDataServerTypes DbServerType { get; set; }

        /// <summary>
        /// Gets or sets the name of the database user. Relevant only when UseTrusted is false.
        /// </summary>
        /// <value>
        /// The database username (e.g.: sa).
        /// </value>
        public string DbUserName
        {
            get
            {
                return UseTrusted ? string.Empty : dbUserName;
            }
            set
            {
                dbUserName = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language to use during the connection.
        /// </value>
        public BoSuppLangs Language { get; set; }

        /// <summary>
        /// Gets or sets the license server. The format should be &lt;server:port&gt;.
        /// </summary>
        /// <value>
        /// License server address in &lt;server:port&gt; format;
        /// </value>
        public string LicenseServer
        {
            get
            {
                return licenseServer;
            }
            set
            {
                licenseServer = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the company password.
        /// </summary>
        /// <value>
        /// Company username password.
        /// </value>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the database server.
        /// </summary>
        /// <value>
        /// The database server. Use the same has your DI API configuration.
        /// </value>
        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the name of the company user.
        /// </summary>
        /// <value>
        /// Company username (e.g.: manager).
        /// </value>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to establish a trusted connection.
        /// </summary>
        /// <value>
        /// Uses windows authentication to establish the connection.
        /// </value>
        public bool UseTrusted { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="DiApiConnectionSettings" /> class from being created.
        /// </summary>
        private DiApiConnectionSettings()
        {
            companyDb = string.Empty;
            dbPassword = string.Empty;
            dbUserName = string.Empty;
            licenseServer = string.Empty;
            password = string.Empty;
            server = string.Empty;
            userName = string.Empty;
            DbServerType = BoDataServerTypes.dst_MSSQL;
            Language = BoSuppLangs.ln_English;
            UseTrusted = false;
        }

        /// <summary>
        /// Creates empty settings.
        /// </summary>
        /// <returns></returns>
        public static DiApiConnectionSettings CreateEmptySettings()
        {
            return new DiApiConnectionSettings();
        }

        /// <summary>
        /// Creates the standard settings.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="dbUserName">Name of the database user.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="userName">Company username.</param>
        /// <param name="password">Company password.</param>
        /// <param name="language">The language.</param>
        /// <param name="licenseServer">The license server in &lt;ServerName:Port&gt; format.</param>
        /// <param name="dbServerType">Type of the database server.</param>
        /// <param name="companyDb">The company database.</param>
        /// <returns></returns>
        public static DiApiConnectionSettings CreateStandardSettings(
            string server,
            string dbUserName,
            string dbPassword,
            string userName,
            string password,
            BoSuppLangs language,
            string licenseServer,
            BoDataServerTypes dbServerType,
            string companyDb)
        {
            return new DiApiConnectionSettings
            {
                Server = server,
                UseTrusted = false,
                DbUserName = dbUserName,
                DbPassword = dbPassword,
                UserName = userName,
                Password = password,
                Language = language,
                LicenseServer = licenseServer,
                DbServerType = dbServerType,
                CompanyDb = companyDb
            };
        }

        /// <summary>
        /// Creates connection settings using trusted connection.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="userName">Company username.</param>
        /// <param name="password">Company password.</param>
        /// <param name="language">The language.</param>
        /// <param name="licenseServer">The license server in &lt;ServerName:Port&gt; format.</param>
        /// <param name="dbServerType">Type of the database server.</param>
        /// <param name="companyDb">The company database.</param>
        /// <returns></returns>
        public static DiApiConnectionSettings CreateTrustedSettings(
            string server,
            string userName,
            string password,
            BoSuppLangs language,
            string licenseServer,
            BoDataServerTypes dbServerType,
            string companyDb)
        {
            return new DiApiConnectionSettings
            {
                Server = server,
                UseTrusted = true,
                UserName = userName,
                Password = password,
                Language = language,
                LicenseServer = licenseServer,
                DbServerType = dbServerType,
                CompanyDb = companyDb
            };
        }

        /// <summary>
        /// Loads from configuration file.
        /// </summary>
        /// <param name="path">The configuration file path.</param>
        /// <returns>
        /// The settings loaded from the file.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown when the file cannot be found.
        /// </exception>
        public static DiApiConnectionSettings Load(string path)
        {
            if (File.Exists(path))
            {
                return Deserialize(path);
            }

            throw new FileNotFoundException($"{path} was not found.");
        }

        /// <summary>
        /// Returns a company object populated with the current settings.
        /// </summary>
        /// <returns>
        /// A pre-populated company object.
        /// </returns>
        public Company ToCompany()
        {
            var company = new Company
            {
                Server = Server,
                UseTrusted = UseTrusted,
                UserName = UserName,
                Password = Password,
                language = Language,
                LicenseServer = LicenseServer,
                DbServerType = DbServerType,
                CompanyDB = CompanyDb
            };

            if (!UseTrusted)
            {
                company.DbUserName = DbUserName;
                company.DbPassword = DbPassword;
            }

            return company;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var properties = GetType().GetProperties();
            var values = new List<string>(properties.Length);

            foreach (var property in properties)
            {
                values.Add($"{property.Name}: " + property.GetValue(this, null));
            }

            return string.Join(Environment.NewLine, values.ToArray());
        }

        /// <summary>
        /// Deserializes the file at the path into a <see cref="DiApiConnectionSettings" />.
        /// </summary>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>
        /// The <see cref="DiApiConnectionSettings" />.
        /// </returns>
        private static DiApiConnectionSettings Deserialize(string path)
        {
            var serializer = new XmlSerializer(typeof(DiApiConnectionSettings));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return (DiApiConnectionSettings) serializer.Deserialize(stream);
            }
        }
    }
}
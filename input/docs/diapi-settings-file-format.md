Description: Details of the DI API settings file format
---
# DI API settings file format

Here is the standard file format for the DI API settings.

```xml
<?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
<DiApiConnectionSettings>
  <Server></Server>
  <UseTrusted></UseTrusted>
  <UserName></UserName>
  <Password></Password>
  <Language></Language>
  <LicenseServer></LicenseServer>
  <DbServerType></DbServerType>
  <CompanyDb></CompanyDb>
  <DbUserName></DbUserName>
  <DbPassword></DbPassword>
</DiApiConnectionSettings>
```

### Server
This should be filled with the name or IP of your server.
```xml
  <Server>localhost</Server>
```

### Use Trusted
This defines whether you'll be using trusted connection or a regular connection. Accepted values are 1 for True and 0 for false.
```xml
  <UseTrusted>1</UseTrusted>
```

### UserName
This is the username you use to connect to the company.
```xml
  <UserName>manager</UserName>
```

### Password
This is the password for the username provided.

### Language
The language to use for this session. Accepted values are the enum literals of BoSuppLang. 
```xml
  <Language>ln_English</Language>
```
### UserName
This is the username you use to connect to the company.
```xml
  <UserName>manager</UserName>
```

### Password
This is the password for the username provided.
### License server
The address of the license server. 
The format must be provided like "server:port". 
```xml
  <LicenseServer>localhost:30000</LicenseServer>
```

### DbServerType
The type of the database server. Accepted values are the enum literals of the BoDataServerTypes.
```xml
  <DbServerType>dst_MSSQL2012</DbServerType>
```

### CompanyDb
The name of the company database.
```xml
  <CompanyDb>SBODEMOUS</CompanyDb>
```
### DbUserName
This is the username you use to connect to your database server.
```xml
  <DbUserName>sa</DbUserName>
```

### DbPassword
This is the password for the database username provided.
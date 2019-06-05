# DI API connection sample

In this sample we will show you how to perform a connection to the DI API using B1PP.

```csharp
string settingsFilePath = args[0];
var settings = DiApiConnectionSettings.Load(settingsFilePath);
var diapi = ConnectionFactory.CreateDiApiConnection(settings);

diapi.Connect();

// something useful is done here

diapi.Disconnect();
```

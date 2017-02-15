# DI API connection sample

In this sample we will show you how to perform a connection to the DI API using B1PP.

```csharp
string settingsFilePath = args[0];
DiApiConnectionSettings settings = DiApiConnectionSettings.Load(settingsFilePath);
IConnection diapi = ConnectionFactory.CreateDiApiConnection(settings);

diapi.Connect();

// something useful is done here

diapi.Disconnect();
```
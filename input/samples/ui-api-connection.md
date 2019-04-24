Description: How to connect to the UI API
---

# UI API connection sample

In this sample, we will show you how to perform a UI API only connection to SAP Business One.
We assume that you are comfortable with Visual Studio and NuGet.

### Pre-requisites
- Visual Studio and NuGet
- Windows Forms project
- Added B1PP via Nuget

### Code
```csharp
var connection = ConnectionFactory.CreateUiApiConnection();
connection.Connect();
```

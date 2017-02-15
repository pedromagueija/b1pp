# Standard connection sample

In this sample, we will show you how to perform a standard connection to SAP Business One (UI and DI API).
We assume that you are comfortable with Visual Studio and NuGet.

### Pre-requisites
- Visual Studio and NuGet
- Windows Forms project
- B1PP.Connections installed using `Install-Package B1PP.Connections -Pre`

### Code
```csharp
namespace StandardConnectionSample
{
    using System;
    using System.Windows.Forms;

    using B1PP.Connections;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            IConnection connection = ConnectionFactory.CreateStandardConnection();
            try
            {
                connection.Connect();
                connection.Application.MessageBox($@"Hello {connection.Application.Company.Name} ! ");
            }
            catch (ConnectionException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK);
            }
            finally
            {
                connection.Disconnect();
            }
        }
    }
}
```
# Standard connection sample

In this sample, we will show you how to perform a standard connection to SAP Business One (UI and DI API).

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

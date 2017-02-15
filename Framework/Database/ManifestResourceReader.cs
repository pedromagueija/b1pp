// <copyright filename="ManifestResourceReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    using System;
    using System.IO;
    using System.Reflection;

    internal class ManifestResourceReader
    {
        private readonly Assembly assembly;

        public ManifestResourceReader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>
        /// The file contents as a string.
        /// </returns>
        /// <exception cref="UnableToReadException">
        /// Thrown when the resource cannot be read.
        /// </exception>
        public string ReadText(string resourceName)
        {
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        string message = $"'{resourceName}' was not found. Was it marked as an Embedded resource?";
                        throw new UnableToReadException(message);
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e) when (
                e is FileLoadException ||
                e is FileNotFoundException ||
                e is BadImageFormatException ||
                e is NotImplementedException
            )
            {
                string message =
                    $@"An error as occurred when attempting to read '{resourceName}'.";
                throw new UnableToReadException(message, e);
            }
        }
    }
}
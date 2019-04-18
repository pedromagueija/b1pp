// <copyright filename="ManifestResourceReader.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP.Exceptions;

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
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        var message = $@"'{resourceName}' was not found. Was it marked as an Embedded resource?";
                        throw new UnableToReadException(message);
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                var message =
                    $@"An error as occurred when attempting to read '{resourceName}'.";
                throw new UnableToReadException(message, e);
            }
        }
    }
}
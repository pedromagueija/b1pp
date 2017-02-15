// <copyright filename="FileContents.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    internal class FileContents
    {
        public int FileSize { get; private set; }
        public byte[] Contents { get; }

        public FileContents(int fileSize, byte[] contents)
        {
            FileSize = fileSize;
            Contents = contents;
        }

        public static implicit operator byte[](FileContents contents)
        {
            return contents.Contents;
        }
    }
}
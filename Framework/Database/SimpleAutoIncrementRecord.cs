// <copyright filename="SimpleAutoIncrementRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    public abstract class SimpleAutoIncrementRecord : ISimpleAutoIncrementRecord
    {
        public int Code { get; }
        public string Name { get; set; }
    }
}
// <copyright filename="SimpleRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    public abstract class SimpleRecord : ISimpleRecord
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
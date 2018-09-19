// <copyright filename="ISimpleAutoIncrementRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Database
{
    public interface ISimpleAutoIncrementRecord
    {
        int Code { get; }
        string Name { get; set; }
    }
}
// <copyright filename="ISimpleRecord.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Database
{
    public interface ISimpleRecord
    {
        string Code { get; set; }
        string Name { get; set; }
    }
}
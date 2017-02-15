// <copyright filename="IFormInstance.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    public interface IFormInstance
    {
        string FormId { get; }
        string FormType { get; }
    }
}
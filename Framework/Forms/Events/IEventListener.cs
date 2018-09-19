// <copyright filename="IEventListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events
{
    internal interface IEventListener : IFilterable
    {
        void Subscribe();

        void Unsubscribe();
    }
}
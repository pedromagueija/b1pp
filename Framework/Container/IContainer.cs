// <copyright filename="IContainer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Container
{
    internal interface IContainer
    {
        void Register<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface;
        TInterface GetInstance<TInterface>() where TInterface : class;
        void Verify();
    }
}
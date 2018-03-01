// <copyright filename="SimpleInjectorContainer.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Container
{
    using SimpleInjector;

    internal class SimpleInjectorContainer : IContainer
    {
        private readonly Container container;

        public SimpleInjectorContainer()
        {
            container = new Container();
        }

        public void Register<TInterface, TImplementation>() where TInterface : class
            where TImplementation : class, TInterface
        {
            container.Register<TInterface, TImplementation>();
        }

        public TInterface GetInstance<TInterface>() where TInterface : class
        {
            return container.GetInstance<TInterface>();
        }

        public void Verify()
        {
            container.Verify();
        }
    }
}
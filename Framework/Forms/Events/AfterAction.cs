// <copyright filename="AfterAction.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;
    using System.Reflection;

    internal class AfterAction<T>
    {
        private AfterAction(ISignature signature, Action<T> handler)
        {
            Handler = handler;
            Signature = signature;
        }

        public string Name
        {
            get
            {
                return Handler.Method.Name;
            }
        }

        private Action<T> Handler { get; }

        private ISignature Signature { get; }

        public static AfterAction<T> CreateNew(ISignature signature, MethodInfo method, object instance)
        {
            return new AfterAction<T>(signature, method.CreateAfterEventDelegate<T>(instance));
        }

        public bool CanHandle(T key)
        {
            return Signature.CanHandle(key);
        }

        public void Handle(T args)
        {
            Handler.Invoke(args);
        }

        public bool IsSame(AfterAction<T> action)
        {
            return Signature.IsSame(action.Signature);
        }
    }
}
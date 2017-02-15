// <copyright filename="BeforeAction.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;
    using System.Reflection;

    internal class BeforeAction<T>
    {
        public string Name
        {
            get
            {
                return Handler.Method.Name;
            }
        }

        private Func<T, bool> Handler { get; }

        private ISignature Signature { get; }

        private BeforeAction(ISignature signature, Func<T, bool> handler)
        {
            Signature = signature;
            Handler = handler;
        }

        public bool CanHandle(T key)
        {
            return Signature.CanHandle(key);
        }

        public static BeforeAction<T> CreateNew(ISignature signature, MethodInfo method, object instance)
        {
            var handler = method.CreateBeforeEventDelegate<T>(instance);
            return new BeforeAction<T>(signature, handler);
        }

        public static BeforeAction<T> CreateNew(ISignature signature, Func<T, bool> handler)
        {
            return new BeforeAction<T>(signature, handler);
        }

        public bool Handle(T args)
        {
            return Handler.Invoke(args);
        }

        public bool IsSame(BeforeAction<T> action)
        {
            return Signature.IsSame(action.Signature);
        }
    }
}
// <copyright filename="HandlersCollection.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System.Collections.Generic;

    internal class HandlersCollection<T>
    {
        public string Id { get; set; }

        private readonly List<AfterAction<T>> after = new List<AfterAction<T>>();

        private readonly List<BeforeAction<T>> before = new List<BeforeAction<T>>();

        public void AddAfter(AfterAction<T> action)
        {
            if (after.Exists(a => a.IsSame(action)))
            {
                throw EventHandlerAlreadyExistsException.CreateFromKey(action.Name);
            }

            after.Add(action);
        }

        public void AddBefore(BeforeAction<T> action)
        {
            if (before.Exists(a => a.IsSame(action)))
            {
                throw EventHandlerAlreadyExistsException.CreateFromKey(action.Name);
            }

            before.Add(action);
        }

        public void HandleAfter(T evt)
        {
            var handler = after.Find(action => action.CanHandle(evt));
            handler?.Handle(evt);
        }

        public void HandleBefore(T evt, out bool bubbleEvent)
        {
            var handler = before.Find(action => action.CanHandle(evt));
            bubbleEvent = handler?.Handle(evt) ?? true;
        }
    }
}
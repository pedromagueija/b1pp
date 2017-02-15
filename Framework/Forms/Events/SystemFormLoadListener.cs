// <copyright filename="SystemFormLoadListener.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events
{
    using System;

    using ItemEvents;

    using SAPbouiCOM;

    internal class SystemFormLoadListener : IItemEventListener
    {
        private readonly Application application;
        private readonly Type classType;

        public string Id { get; }

        public SystemFormLoadListener(Application application, string formType, Type classType)
        {
            this.application = application;
            this.classType = classType;
            Id = formType;
        }

        public void OnItemEvent(ref ItemEvent e, out bool bubbleEvent)
        {
            if (e.EventType == BoEventTypes.et_FORM_LOAD && e.BeforeAction && e.FormTypeEx.Equals(Id))
            {
                Form form = application.Forms.Item(e.FormUID);
                var instance = (ISystemFormInstance) Activator.CreateInstance(classType, form);

                instance.Initialize();
            }

            bubbleEvent = true;
        }
    }
}
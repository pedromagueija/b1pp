// <copyright filename="ItemEventSignature.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SAPbouiCOM;

    internal class ItemEventSignature : ISignature
    {
        public bool BeforeAction { get; set; }

        public List<BoFormMode> Modes { get; set; }

        public List<string> ColumnIds { get; set; }

        public string ItemId { get; set; }

        public BoEventTypes EventType { get; set; }

        public bool CanHandle<T>(T args)
        {
            var itemEventArgs = args as ItemEvent;
            if (itemEventArgs == null)
            {
                return false;
            }

            return CanHandle(itemEventArgs);
        }

        public bool IsSame(ISignature other)
        {
            var signature = other as ItemEventSignature;
            if (signature == null)
            {
                return false;
            }

            return IsSame(signature);
        }

        public static ItemEventSignature Create(MethodInfo method)
        {
            var itemEvent = method.GetAttribute<ItemEventHandlerAttribute>();
            var columns = method.GetAttribute<ColumnsAttribute>() ?? ColumnsAttribute.None;
            var modes = method.GetAttribute<FormModesAttribute>() ?? FormModesAttribute.Default;

            return new ItemEventSignature
            {
                EventType = itemEvent.EventType,
                ItemId = itemEvent.ItemId,
                ColumnIds = new List<string>(columns.ColumnIds),
                Modes = new List<BoFormMode>(modes.Modes),
                BeforeAction = itemEvent.BeforeAction
            };
        }

        private bool CanHandle(ItemEvent arg)
        {
            if (BeforeAction != arg.BeforeAction)
            {
                return false;
            }

            if (EventType != arg.EventType)
            {
                return false;
            }

            if (!ItemId.Equals(arg.ItemUID))
            {
                return false;
            }

            if (Modes.Count > 0 && !Modes.Contains((BoFormMode) arg.FormMode))
            {
                return false;
            }

            if (ColumnIds.Count > 0 && !ColumnIds.Contains(arg.ColUID))
            {
                return false;
            }

            return true;
        }

        private bool IsSame(ItemEventSignature other)
        {
            if (BeforeAction != other.BeforeAction)
            {
                return false;
            }

            if (EventType != other.EventType)
            {
                return false;
            }

            if (!ItemId.Equals(other.ItemId))
            {
                return false;
            }

            if (Modes.Count > 0 && !Modes.Intersect(other.Modes).Any())
            {
                return false;
            }

            if (ColumnIds.Count > 0 && !ColumnIds.Intersect(other.ColumnIds).Any())
            {
                return false;
            }

            return true;
        }
    }
}
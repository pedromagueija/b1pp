// <copyright filename="ColumnsAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Allows you to set which columns will the method run for.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    ///     <code>
    /// [ItemEventHandler(@"itemId", BoEventTypes.et_MATRIX_LINK_PRESSED)]
    /// [Columns(@"columnId")]
    /// private void OnAfterColumnLinkPressed(ItemEvent e)
    /// {
    ///   // the code in this method only runs for columnId;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public class ColumnsAttribute : Attribute
    {
        /// <summary>
        /// Represents no columns.
        /// </summary>
        public static readonly ColumnsAttribute None = new ColumnsAttribute();

        /// <summary>
        /// Stores the column id list set by the user.
        /// </summary>
        private IEnumerable<string> columnIds;

        /// <summary>
        /// Gets the column ids.
        /// </summary>
        public IEnumerable<string> ColumnIds
        {
            get
            {
                return new List<string>(columnIds);
            }
            private set
            {
                columnIds = new List<string>(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsAttribute" /> class.
        /// </summary>
        /// <param name="values">The column ids.</param>
        /// <remarks>
        /// Any empty id will be ignored.
        /// </remarks>
        public ColumnsAttribute(params string[] values)
        {
            ColumnIds = values?.Where(id => !string.IsNullOrWhiteSpace(id)) ?? new List<string>();
        }
    }
}
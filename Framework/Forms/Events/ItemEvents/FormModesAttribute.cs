// <copyright filename="FormModesAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Forms.Events.ItemEvents
{
    using System;
    using System.Collections.Generic;

    using SAPbouiCOM;

    [AttributeUsage(AttributeTargets.Method)]
    public class FormModesAttribute : Attribute
    {
        /// <summary>
        /// The default forms modes: Add, Ok and Update.
        /// </summary>
        public static readonly FormModesAttribute Default = new FormModesAttribute(
            BoFormMode.fm_ADD_MODE,
            BoFormMode.fm_UPDATE_MODE,
            BoFormMode.fm_OK_MODE);

        private readonly List<BoFormMode> modes;

        public List<BoFormMode> Modes => new List<BoFormMode>(modes);

        /// <summary>
        /// Initializes a new instance of the <see cref="FormModesAttribute" /> class.
        /// </summary>
        /// <param name="modes">The modes.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="modes" /> is <see langword="null" />.
        /// </exception>
        public FormModesAttribute(params BoFormMode[] modes)
        {
            if (modes == null)
            {
                throw new ArgumentNullException(nameof(modes));
            }

            this.modes = new List<BoFormMode>(modes);
        }

        public FormModesAttribute()
        {
            modes = new List<BoFormMode>
            {
                BoFormMode.fm_ADD_MODE,
                BoFormMode.fm_EDIT_MODE,
                BoFormMode.fm_FIND_MODE,
                BoFormMode.fm_OK_MODE,
                BoFormMode.fm_PRINT_MODE,
                BoFormMode.fm_UPDATE_MODE,
                BoFormMode.fm_VIEW_MODE
            };
        }
    }
}
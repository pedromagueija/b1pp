// <copyright filename="MultipleDoubleValuesArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System.Collections.Generic;
    using System.Linq;

    internal class MultipleDoubleValuesArg : QueryArgBase
    {
        private readonly IEnumerable<double> values;

        public override string Value
        {
            get
            {
                var arguments = string.Join(",", values.Select(ToSqlParameter));

                return arguments;
            }
        }

        public MultipleDoubleValuesArg(string placeHolder, IEnumerable<double> values)
        {
            this.values = values;
            PlaceHolder = placeHolder;
        }
    }
}
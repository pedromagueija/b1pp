// <copyright filename="MultipleIntValuesArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System.Collections.Generic;
    using System.Linq;

    internal class MultipleIntValuesArg : QueryArgBase
    {
        private readonly IEnumerable<int> values;

        public override string Value
        {
            get
            {
                string arguments = string.Join(",", values.Select(ToSqlParameter));

                return arguments;
            }
        }

        public MultipleIntValuesArg(string placeHolder, IEnumerable<int> values)
        {
            this.values = values;
            PlaceHolder = placeHolder;
        }
    }
}
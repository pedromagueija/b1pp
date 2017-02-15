// <copyright filename="MultipleStringValuesArg.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System.Collections.Generic;
    using System.Linq;

    internal class MultipleStringValuesArg : QueryArgBase
    {
        private readonly IEnumerable<string> values;

        public override string Value
        {
            get
            {
                var stringValues = new List<string>(values.Count());
                stringValues.AddRange(values.Select(ToSqlParameter));
                return string.Join(",", stringValues.ToArray());
            }
        }

        public MultipleStringValuesArg(string placeHolder, IEnumerable<string> values)
        {
            this.values = values;
            PlaceHolder = placeHolder;
        }
    }
}
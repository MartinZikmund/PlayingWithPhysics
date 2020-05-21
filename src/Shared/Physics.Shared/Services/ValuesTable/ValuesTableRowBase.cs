using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Services.ValuesTable
{
    public abstract class ValuesTableRowBase : IValuesTableRow
    {
        private const char TabSeparator = '\t';

        public string ToTabString()
        {
            var builder = new StringBuilder();
            foreach (var item in GetCellValuesInOrder())
            {
                if (builder.Length != 0)
                {
                    builder.Append(TabSeparator);
                }
                builder.Append(item);
            }
            return builder.ToString();
        }

        protected abstract IEnumerable<string> GetCellValuesInOrder();
    }
}

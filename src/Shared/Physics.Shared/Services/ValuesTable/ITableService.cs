using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Services.ValuesTable
{
    public interface ITableService<TValuesTableRow>
        where TValuesTableRow : IValuesTableRow
    {
        IEnumerable<TValuesTableRow> CalculateTable(float timeInterval);


    }
}

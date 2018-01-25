using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Inventory_mvc.DAO
{
    public interface IChartDAO
    {
        void RetrieveQty(DateTime? date);
    }
}
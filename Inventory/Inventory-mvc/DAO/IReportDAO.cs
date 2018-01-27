using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    interface IReportDAO
    {
        List<Purchase_Detail> RetrieveQty(DateTime ds, DateTime de);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.DAO;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    interface IReportService
    {
        List<ReportViewModel> RetrieveQty();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    interface IReportService
    {
        //List<ReportViewModel> RetrieveQty(DateTime ds, DateTime de);

        List<ReportViewModel> GetItemRequestTrend(string categoryID, string itemCode, int[] years);

        List<ReportViewModel> GetApprovedRequisitionsOfYear(int year);


        List<int> GetSelectableYears(int baseYear);

        List<ReportViewModel> GetApprovedRequisitionDetialsBasedOnYearAndMonth(int year, int month);

        // TODO - REMOVE THIS METHOD
        void GenerateRandomDataForRequisitionRecords();

        List<int> GetSelectableMonths(int year);

        int GetEarliestYear();

        List<ReportViewModel> GetDepartmentApprovedRequisitionsOfYear(int year, string deptCode);
    }
}

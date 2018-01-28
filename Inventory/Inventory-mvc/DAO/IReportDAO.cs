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

        List<Requisition_Detail> GetApprovedRequisitionDetailsByItemCodeAndYear(string itemCode, int[] years);

        List<Requisition_Detail> GetApprovedRequisitionDetailsOfYear(int year);

        List<Requisition_Detail> GetApprovedRequisitionDetialsBasedOnYearAndMonth(int year, int month);
        int GetEarliestYear();

        List<Requisition_Detail> GetApprovedRequisitionDetailsByCriteria(string categoryID, string itemCode, int[] years);
    }
}

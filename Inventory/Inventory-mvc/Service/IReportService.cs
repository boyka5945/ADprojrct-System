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
        List<ReportViewModel> GetItemRequestTrend(string categoryID, string itemCode, int[] years);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all </param>
        /// <param name="itemCode"> "-1" for all </param>
        /// <param name="deptCode"> null for all </param>
        /// <param name="years"> null for all </param>
        /// <param name="months"> null for all </param>
        /// <returns></returns>
        List<ReportViewModel> GetApprovedRequisitionDetialsBasedCriteria(string categoryID, string itemCode, string deptCode, int[] years, int[] months);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all </param>
        /// <param name="itemCode"> "-1" for all </param>
        /// <param name="deptCode"> null for all </param>
        /// <param name="years"> null for all </param>
        /// <param name="months"> null for all </param>
        /// <returns></returns>
        List<ReportViewModel> GetReorderAmountBasedOnCriteria(string categoryID, string itemCode, string supplierCode, int[] years, int[] months);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all </param>
        /// <param name="itemCode"> "-1" for all </param>
        /// <param name="supplierCode"> null for all </param>
        /// <param name="yearAndMonths"> eg "5-2017" for May 2017 </param>
        /// <returns></returns>
        List<ReportViewModel> GetReorderAmountBasedOnCriteria(string categoryID, string itemCode, string supplierCode, string[] yearAndMonths);


        List<int> GetSelectableYears(int baseYear);

        List<int> GetSelectableMonths(int year);

        int GetEarliestYear();

    }
}

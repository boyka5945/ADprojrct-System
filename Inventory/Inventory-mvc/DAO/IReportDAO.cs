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
        int GetEarliestYear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all categories </param>
        /// <param name="itemCode"> "-1" for all items </param>
        /// <param name="deptCode"> null for all departments </param>
        /// <param name="years"> null for all </param>
        /// <param name="months"> null for all </param>
        /// <returns></returns>
        List<Requisition_Detail> GetApprovedRequisitionDetailsByCriteria(string categoryID, string itemCode, string deptCode, int[] years, int[] months);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all categories </param>
        /// <param name="itemCode"> "-1" for all items </param>
        /// <param name="supplierCode"> null for all suppliers </param>
        /// <param name="years"> null for all </param>
        /// <param name="months"> null for all </param>
        /// <returns></returns>
        List<Purchase_Detail> GetPurchaseDetailsByCriteria(string categoryID, string itemCode, string supplierCode, int[] years, int[] months);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all </param>
        /// <param name="itemCode"> "-1" for all </param>
        /// <param name="supplierCode"> null for all </param>
        /// <param name="yearAndMonths"> eg "5-2017" for May 2017 </param>
        /// <returns></returns>
        List<Purchase_Detail> GetPurchaseDetailsByCriteria(string categoryID, string itemCode, string supplierCode, string[] yearAndMonths);
    }
}

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

        int GetEarliestYear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"> "-1" for all categories </param>
        /// <param name="itemCode"> "-1" for all items </param>
        /// <param name="deptCode"> null for all departments </param>
        /// <param name="years"> null for all </param>
        /// <param name="months"> nul for all </param>
        /// <returns></returns>
        List<Requisition_Detail> GetApprovedRequisitionDetailsByCriteria(string categoryID, string itemCode, string deptCode, int[] years, int[] months);

    }
}

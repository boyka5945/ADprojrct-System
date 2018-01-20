using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    interface IAdjustmentVoucherService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vmList"></param>
        /// <param name="remarks"> 
        /// 0: Inventory_Check | 1: Stock_Retrieval | 2: Disbursement | 3: Reconciliation
        /// </param>
        /// <returns></returns>
        bool SubmitNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, int remarks, string requesterID);

        bool ValidateNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, out string errorMessage);

        List<AdjustmentVoucherViewModel> GetVoucherRecordsByCriteria(string approverID, string status, string sortOrder);

        Adjustment_Voucher_Record FindVoucherRecordByVoucherNo(int voucherNo);
    }
}

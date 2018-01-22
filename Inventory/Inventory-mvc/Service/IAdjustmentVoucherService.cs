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
        /// AdjustmentVoucherRemarks => INV_CHECK | STK_RETRIVE | DISBURSE | RECONCILE
        /// </param>
        /// <returns></returns>
        bool SubmitNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, string remarks, string requesterID);

        bool ValidateNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, out string errorMessage);

        List<AdjustmentVoucherViewModel> GetVoucherRecordsByCriteria(string approverID, string status, string sortOrder);

        Adjustment_Voucher_Record FindVoucherRecordByVoucherNo(int voucherNo);

        List<AdjustmentVoucherViewModel> IsUserAuthorizedToViewVoucherDetail(int voucherNo, string userID, out string errorMessage);

        decimal GetVoucherRecordTotalAmount(int voucherNo);

        decimal GetVoucherRecordTotalAmount(Adjustment_Voucher_Record record);

        bool ValidateAdjustmentVoucherBeforeApprove(int voucherNo, out string errorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="approverID"></param>
        /// <param name="remark"> Email content </param>
        /// <returns></returns>
        bool RejectVoucherRecord(int voucherNo, string approverID, string remark);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="approverID"></param>
        /// <param name="remark"> Email content </param>
        /// <returns></returns>
        bool ApproveVoucherRecord(int voucherNo, string approverID, string remark);

        int GetPendingVoucherCount();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class AdjustmentVoucherService : IAdjustmentVoucherService
    {
        enum Remarks
        {
            Inventory_Check, Stock_Retrieval, Disbursement, Reconciliation
        }

        enum Status
        {
            Pending, Approved, Rejected
        }

        IAdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();

        public bool SubmitNewAdjustmentVoucher(List<NewVoucherViewModel> vmList, int remarks, string requesterID)
        {            
            Adjustment_Voucher_Record vourcherRecord = new Adjustment_Voucher_Record();
            vourcherRecord.issueDate = DateTime.Today;
            vourcherRecord.handlingStaffID = requesterID;
            vourcherRecord.status = Enum.GetName(typeof(Status), Status.Pending);
            vourcherRecord.remarks = Enum.GetName(typeof(Remarks), remarks);

            List<Voucher_Detail> details = new List<Voucher_Detail>();
            foreach(var vm in vmList)
            {
                if(vm.Quantity != 0) // ignore quantity which is equal to 0
                {
                    Voucher_Detail detail = new Voucher_Detail();
                    detail.itemCode = vm.ItemCode;
                    detail.adjustedQty = vm.Quantity;
                    detail.remarks = vm.Reason;
                    details.Add(detail);
                }
            }

            vourcherRecord.Voucher_Details = details;

            return adjustmentVoucherDAO.AddNewAdjustmentVoucher(vourcherRecord);
        }
    }
}
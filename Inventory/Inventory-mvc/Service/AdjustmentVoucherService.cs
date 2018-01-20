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
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();

        public bool SubmitNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, int remarks, string requesterID)
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

        public bool ValidateNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, out string errorMessage)
        {
            errorMessage = null;

            foreach(var item in vmList)
            {
                Stationery s = stationeryService.FindStationeryByItemCode(item.ItemCode);
                if((s.stockQty + item.Quantity) < 0)
                {
                    errorMessage = String.Format("Negative adjustment quantity of {0} is greater than current stock.", item.ItemCode);
                    return false;
                }
            }
            return true;
        }

        public List<AdjustmentVoucherViewModel> GetVoucherRecordsByCriteria(string approverID, string status, string sortOrder)
        {
            bool isManager = userService.IsStoreManager(approverID);
            bool isSupervisor = userService.IsStoreSupervisor(approverID);

            List<Adjustment_Voucher_Record> records = adjustmentVoucherDAO.GetVouchersByCriteria(status, sortOrder);

            List<AdjustmentVoucherViewModel> vmList = new List<AdjustmentVoucherViewModel>();

            if (status == "Pending")
            {
                // calculate total amount
                // return sorted voucher records based on total Amount & user role
                // below 250 = supervisor | above 250 = manager
                foreach (var record in records)
                {
                    AdjustmentVoucherViewModel vm = ConvertRecordToViewModel(record);
                    vm.VoucherTotalAmount = GetVoucherRecordTotalAmount(record.voucherID); // only Pending need total amount

                    if(isManager && vm.VoucherTotalAmount > 250)
                    {
                        // manager for voucher amount > 250
                        vmList.Add(vm);
                    }
                    else if(isSupervisor && vm.VoucherTotalAmount <= 250)
                    {
                        // supervisor for voucher amount <= 250
                        vmList.Add(vm);
                    }
                }
            }
            else // status == Approved / Rejected
            {
                // return sorted voucher records for other status
                // both supervisor and manager can retrieve all records
                foreach (var record in records)
                {
                    AdjustmentVoucherViewModel vm = ConvertRecordToViewModel(record);
                    vmList.Add(vm);
                }
            }

            return vmList;
        }

        public Adjustment_Voucher_Record FindVoucherRecordByVoucherNo(int voucherNo)
        {
            return adjustmentVoucherDAO.FindByVoucherID(voucherNo);
        }

        private AdjustmentVoucherViewModel ConvertRecordToViewModel(Adjustment_Voucher_Record record)
        {
            AdjustmentVoucherViewModel vm = new AdjustmentVoucherViewModel();

            vm.VoucherNo = record.voucherID;
            vm.Requester = userService.FindNameByID(record.handlingStaffID);
            vm.IssueDate = record.issueDate;
            vm.VoucherTotalAmount = 0.00M; // only Pending need this info

            return vm;
        }


        private AdjustmentVoucherViewModel ConvertDetailToViewModel(Voucher_Detail detail)
        {
            Stationery stationery = stationeryService.FindStationeryByItemCode(detail.itemCode);
            AdjustmentVoucherViewModel vm = new AdjustmentVoucherViewModel();

            // Stationery information
            vm.ItemCode = detail.itemCode;
            vm.StationeryDescription = stationery.description;
            vm.UOM = stationery.unitOfMeasure;
            vm.Price = stationery.price;

            // Voucher Record information
            vm.VoucherNo = detail.voucherID;
            vm.Requester = userService.FindNameByID(detail.Adjustment_Voucher_Record.handlingStaffID); 
            vm.VoucherTotalAmount = 0.00M;
            vm.IssueDate = detail.Adjustment_Voucher_Record.issueDate;

            // Voucher Detail information
            vm.Quantity = detail.adjustedQty;
            vm.Reason = detail.remarks;

            return vm;
        }

        private decimal GetVoucherRecordTotalAmount(int voucherNo)
        {
            Adjustment_Voucher_Record record = FindVoucherRecordByVoucherNo(voucherNo);

            decimal totalAmount = 0.00M;
            foreach (var detail in record.Voucher_Details)
            {
                if (detail.adjustedQty < 0)
                {
                    Stationery stationery = stationeryService.FindStationeryByItemCode(detail.itemCode);
                    totalAmount += detail.adjustedQty * stationery.price;
                }
            }

            return totalAmount;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.DAO;
using Inventory_mvc.Function;
using Inventory_mvc.Utilities;
using System.Transactions;

namespace Inventory_mvc.Service
{
    public class AdjustmentVoucherService : IAdjustmentVoucherService
    {

        IAdjustmentVoucherDAO adjustmentVoucherDAO = new AdjustmentVoucherDAO();
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();
        ITransactionRecordService transactionService = new TransactionRecordService();

        public bool SubmitNewAdjustmentVoucher(List<AdjustmentVoucherViewModel> vmList, string remarks, string requesterID)
        {            
            Adjustment_Voucher_Record vourcherRecord = new Adjustment_Voucher_Record();
            vourcherRecord.issueDate = DateTime.Today;
            vourcherRecord.handlingStaffID = requesterID;
            vourcherRecord.status = AdjustmentVoucherStatus.PENDING;
            vourcherRecord.remarks = remarks;

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
            decimal voucherAmount = GetVoucherRecordTotalAmount(vourcherRecord);

            adjustmentVoucherDAO.AddNewAdjustmentVoucher(vourcherRecord);
            EmailNotification.EmailNotificationForNewAdjustmentVoucher(requesterID, voucherAmount);
            return true;
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

                if(item.Quantity == 0)
                {
                    errorMessage = String.Format("Quantity of {0} is 0.", item.ItemCode);
                    return false;
                }
            }
            return true;
        }

        public bool ValidateAdjustmentVoucherBeforeApprove(int voucherNo, out string errorMessage)
        {
            errorMessage = null;

            Adjustment_Voucher_Record record = FindVoucherRecordByVoucherNo(voucherNo);

            foreach (var item in record.Voucher_Details)
            {
                Stationery s = stationeryService.FindStationeryByItemCode(item.itemCode);
                if ((s.stockQty + item.adjustedQty) < 0)
                {
                    errorMessage = String.Format("Cannot process voucher due to negative adjustment quantity of {0} is greater than current stock.", item.itemCode);
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

            if (status == AdjustmentVoucherStatus.PENDING)
            {
                // calculate total amount
                // return sorted voucher records based on total Amount & user role
                // below 250 = supervisor | above 250 = manager
                foreach (var record in records)
                {
                    AdjustmentVoucherViewModel vm = ConvertRecordToViewModel(record);
                    vm.VoucherTotalAmount = GetVoucherRecordTotalAmount(record.voucherID); // only Pending need total amount

                    if(isManager && vm.VoucherTotalAmount * -1 > 250)
                    {
                        // manager => voucher amount > 250
                        vmList.Add(vm);
                    }
                    else if((isSupervisor && vm.VoucherTotalAmount * -1 <= 250) || isManager)
                    {
                        // supervisor, manager => voucher amount <= 250
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




        /// <summary>
        /// Return voucher details if user is authorized to view the record
        /// </summary>
        /// <param name="voucherNo"></param>
        /// <param name="userID"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<AdjustmentVoucherViewModel> IsUserAuthorizedToViewVoucherDetail(int voucherNo, string userID, out string errorMessage)
        {
            errorMessage = null;
            List<AdjustmentVoucherViewModel> vmList = new List<AdjustmentVoucherViewModel>();

            Adjustment_Voucher_Record record = FindVoucherRecordByVoucherNo(voucherNo);
            if(record == null)
            {                
                errorMessage = String.Format("Non-existing voucher.");
                return null;
            }


            decimal voucherTotalAmount = GetVoucherRecordTotalAmount(record);

            if(record.status == AdjustmentVoucherStatus.PENDING)
            {
                // check for current user
                bool isManager = userService.IsStoreManager(userID);
                bool isSupervisor = userService.IsStoreSupervisor(userID);

                if (isSupervisor && !(voucherTotalAmount * -1 <= 250))
                {
                    errorMessage = String.Format("You have not right to approve this voucher.");
                    return null;
                }
            }

            foreach (var detail in record.Voucher_Details)
            {
                AdjustmentVoucherViewModel vm = ConvertDetailToViewModel(detail);
                vm.VoucherTotalAmount = voucherTotalAmount;
                vmList.Add(vm);
            }

            return vmList;
        }

        public decimal GetVoucherRecordTotalAmount(int voucherNo)
        {
            // to retrieve voucher_details associated with this voucherNo
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

        public decimal GetVoucherRecordTotalAmount(Adjustment_Voucher_Record record)
        {
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


        public bool RejectVoucherRecord(int voucherNo, string approverID, string remark)
        {
            Adjustment_Voucher_Record record = FindVoucherRecordByVoucherNo(voucherNo);
            record.authorisingStaffID = approverID;
            record.status = AdjustmentVoucherStatus.REJECTED;
            record.approvalDate = DateTime.Today;

            adjustmentVoucherDAO.UpdateAdjustmentVoucherInfo(record);

            EmailNotification.EmailNotificatioForAdjustmentVoucherApprovalStatus(voucherNo, AdjustmentVoucherStatus.REJECTED, remark);
            return true;
        }

        public bool ApproveVoucherRecord(int voucherNo, string approverID, string remark)
        {
            bool result = false;

            // Update Adjustment_Voucher_Record
            Adjustment_Voucher_Record voucherRecord = FindVoucherRecordByVoucherNo(voucherNo);
            voucherRecord.authorisingStaffID = approverID;
            voucherRecord.status = AdjustmentVoucherStatus.APPROVED;
            voucherRecord.approvalDate = DateTime.Today;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    adjustmentVoucherDAO.UpdateAdjustmentVoucherInfo(voucherRecord);

                    // Update Stationery Quantity
                    foreach (Voucher_Detail detail in voucherRecord.Voucher_Details)
                    {
                        // throw Exception if stationery quantity become zero
                        stationeryService.UpdateStationeryQuantity(detail.itemCode, detail.adjustedQty);
                    }

                    // Insert Record into Transaction table
                    Transaction_Record transRecord = new Transaction_Record();
                    transRecord.clerkID = voucherRecord.handlingStaffID;
                    transRecord.date = voucherRecord.approvalDate;
                    transRecord.type = TransactionTypes.STOCK_ADJUSTMENT;

                    transRecord.Transaction_Details = new List<Transaction_Detail>();
                    foreach (Voucher_Detail voucherDetail in voucherRecord.Voucher_Details)
                    {
                        Transaction_Detail transDetail = new Transaction_Detail();
                        transDetail.itemCode = voucherDetail.itemCode;
                        transDetail.adjustedQty = voucherDetail.adjustedQty;
                        transDetail.balanceQty = stationeryService.FindStationeryByItemCode(voucherDetail.itemCode).stockQty; // stock after adjustment
                        transDetail.remarks = String.Format("Voucher no.: {0} ({1})", voucherRecord.voucherID, voucherRecord.remarks);

                        transRecord.Transaction_Details.Add(transDetail);
                    }
                    // throw Exception if error occur when writing to database 
                    transactionService.AddNewTransactionRecord(transRecord);

                    ts.Complete();
                    result = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            if (result)
            {
                // send email notification
                EmailNotification.EmailNotificatioForAdjustmentVoucherApprovalStatus(voucherNo, AdjustmentVoucherStatus.APPROVED, remark);
            }

            return result;
        }



        private AdjustmentVoucherViewModel ConvertRecordToViewModel(Adjustment_Voucher_Record record)
        {
            AdjustmentVoucherViewModel vm = new AdjustmentVoucherViewModel();

            vm.VoucherNo = record.voucherID;
            vm.Requester = userService.FindNameByID(record.handlingStaffID);
            vm.IssueDate = record.issueDate;
            vm.VoucherTotalAmount = 0.00M; // only Pending need this info
            vm.ApprovalDate = record.approvalDate;
            if(!String.IsNullOrEmpty(record.authorisingStaffID))
            {
                vm.Approver = userService.FindNameByID(record.authorisingStaffID);
            }
            vm.Causes = record.remarks;
            vm.Status = record.status;

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
            vm.ApprovalDate = detail.Adjustment_Voucher_Record.approvalDate;
            if (!String.IsNullOrEmpty(detail.Adjustment_Voucher_Record.authorisingStaffID))
            {
                vm.Approver = userService.FindNameByID(detail.Adjustment_Voucher_Record.authorisingStaffID);
            }
            vm.Causes = detail.Adjustment_Voucher_Record.remarks;
            vm.Status = detail.Adjustment_Voucher_Record.status;


            // Voucher Detail information
            vm.Quantity = detail.adjustedQty;
            vm.Reason = detail.remarks;

            return vm;
        }

        public int GetPendingVoucherCount()
        {
            return adjustmentVoucherDAO.GetPendingVoucherCount();
        }
    }
}
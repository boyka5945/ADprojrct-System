using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Function;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Service
{
    public class RequisitionRecordService : IRequisitionRecordService
    {
        IRequisitionRecordDAO rDAO = new RequisitionRecordDAO();
        IUserService userService = new UserService();

        public List<Requisition_Record> GetAllRequisition()
        {
            return rDAO.GetAllRequisition();
        }

        public List<Requisition_Record> GetRequisitionRecordByDept(string deptCode)
        {           
            return rDAO.GetRequisitionRecordByDept(deptCode);
        }

        public Requisition_Record GetRequisitionByID(int id)
        {
            return rDAO.FindByRequisitionNo(id);
        }

        public List<Requisition_Detail> GetDetailsByNo(int No = 0)
        {
            return rDAO.GetDetailsByNO(No);
        }

        public void UpdateRequisition(Requisition_Record rr, string status, string approveStaffID)
        {
            rDAO.UpdateRequisition(rr, status, approveStaffID);

        }

        public List<string> GetItemCodeList()
        {
            return rDAO.GetDetailsByGroup();
        }

        public List<Requisition_Record> GetRecordByItemCode(string itemCode)
        {
            return rDAO.GetRecordByItemCode(itemCode);
        }

        public int? FindUnfulfilledQtyBy2Key(string itemCode, int requisitionNo)
        {
            return rDAO.FindUnfulfilledQtyBy2Key(itemCode, requisitionNo);
        }

        public Requisition_Detail FindDetailsBy2Key(string itemCode, int requisitionNo)
        {
            return rDAO.FindDetailsBy2Key(itemCode, requisitionNo);
        }

        public void UpdateDetails(string itemcode, int requisitionNo, int? allocateQty)
        {
            rDAO.UpdateRequisitionDetails(itemcode, requisitionNo, allocateQty);
        }

        public void UpdateDetailsAndroid(string itemcode, int requisitionNo, int? allocateQty)
        {
            rDAO.UpdateRequisitionDetailsAndroid(itemcode, requisitionNo, allocateQty);
        }


        public bool SubmitNewRequisition(Requisition_Record requisition, string requesterID)
        {
            requisition.requesterID = requesterID;
            requisition.deptCode = userService.FindDeptCodeByID(requesterID);
            requisition.status = RequisitionStatus.PENDING_APPROVAL;
            requisition.requestDate = DateTime.Today;

            try
            {
                rDAO.SubmitNewRequisition(requisition);
                // send email notification
                EmailNotification.EmailNotificatioForNewRequisition(requisition.requesterID);

                return true;
            }
            catch (EmailException e)
            {
                throw new EmailException(e.Message);
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
        }

        // TODO - REMOVE THIS METHOD
        public bool GenerateRandomRequisition(Requisition_Record requisition, string requesterID, DateTime date)
        {
            requisition.requesterID = requesterID;
            requisition.deptCode = userService.FindDeptCodeByID(requesterID);
            requisition.requestDate = date;

            try
            {
                rDAO.SubmitNewRequisition(requisition);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public List<Requisition_Record> GetRecordsByRequesterID(string requesterID)
        {
            return rDAO.GetRecordsByRequesterID(requesterID);
        }

        public bool ValidateRequisition(Requisition_Record requisition)
        {
            if (requisition.Requisition_Detail.Count == 0)
            {
                return false;
            }

            foreach (Requisition_Detail requestItem in requisition.Requisition_Detail)
            {
                if (requestItem.qty < 1 || String.IsNullOrEmpty(requestItem.itemCode))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsUserValidToSubmitRequisition(string requesterID)
        {
            int userRole = userService.GetRoleByID(requesterID);

            List<int> disallowedRoles = new List<int>();
            disallowedRoles.Add((int)UserRoles.RoleID.ActingDepartmentHead);
            disallowedRoles.Add((int)UserRoles.RoleID.DepartmentHead);
            disallowedRoles.Add((int)UserRoles.RoleID.StoreManager);

            return (!disallowedRoles.Contains(userRole));
        }

        public List<Disbursement> GetRequisitionByDept(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetRequisitionByDept(deptCode);
        }

        public List<Requisition_Detail> GetAllRequisitionByDept(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetAllRequisitionByDept(deptCode);
        }

        public List<Requisition_Detail> GetPendingRequestByDeptCode(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetPendingRequestByDeptCode(deptCode);
        }

        public List<Disbursement> GetPendingDisbursementByDept(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetPendingDisbursementByDept(deptCode);
        }

        public List<Requisition_Detail> GetAllPendingDisbursementByDept(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetAllPendingDisbursementByDept(deptCode);
        }

        public List<RetrieveForm> GetRetrieveFormByDateTime(DateTime? time)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetRetrieveFormByDateTime(time);
        }
        public bool DeleteRequisition(int recordNo)
        {
            return rDAO.DeleteRequisition(recordNo);
        }

        public bool ValidateUser(int requisitionNo, string requesterID)
        {
            Requisition_Record record = rDAO.FindByRequisitionNo(requisitionNo);

            return (record.requesterID == requesterID);
        }

        /// <summary>
        /// Return record if the record is raised by the user. Return null if false
        /// </summary>
        /// <param name="requisitionNo"></param>
        /// <param name="requesterID"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Requisition_Record IsUserAuthorizedForRequisition(int requisitionNo, string requesterID, out string errorMessage)
        {
            Requisition_Record record = null;
            errorMessage = null;

            record = GetRequisitionByID(requisitionNo);

            if (record == null)
            {
                errorMessage = String.Format("Non-existing requisition.");
            }
            else if (record.requesterID != requesterID)
            {
                errorMessage = String.Format("You have not right to access.");
            }

            return record;
        }



        public bool UpdateRequisitionDetails(List<RequisitionDetailViewModel> vmList, out string errorMessage)
        {
            errorMessage = null;

            foreach (var vm in vmList)
            {
                Requisition_Detail rd = new Requisition_Detail();
                rd.itemCode = vm.ItemCode;
                rd.requisitionNo = vm.RequisitionNo;
                rd.qty = vm.RequestQty;

                if (!rDAO.UpdateRequisitionDetails(rd))
                {
                    errorMessage = String.Format("Error occured when updating the quantity of {0}", vm.Description);
                    return false;
                }
            }

            return true;
        }

        public List<Requisition_Record> GetSortedRecordsByRequesterID(string requesterID, string sortOrder)
        {
            return rDAO.GetSortedRecordsByRequesterID(requesterID, sortOrder);
        }

        public int DetailsCountOfOneItemcode(string itemcode)
        {
            return rDAO.DetailsCountOfOneItemcode(itemcode);
        }

        public List<RequisitionDetailViewModel> GetViewModelFromRequisitionRecord(Requisition_Record record)
        {
            List<RequisitionDetailViewModel> vmList = new List<RequisitionDetailViewModel>();

            foreach (var item in record.Requisition_Detail)
            {
                RequisitionDetailViewModel vm = new RequisitionDetailViewModel();

                vm.ItemCode = item.itemCode;
                vm.RequestQty = (item.qty == null) ? 0 : (int)item.qty;
                vm.RequisitionNo = record.requisitionNo;
                vm.ReceivedQty = (item.fulfilledQty == null) ? 0 : (int)item.fulfilledQty;
                vm.UOM = item.Stationery.unitOfMeasure;
                vm.Description = item.Stationery.description;
                vm.RequestDate = (DateTime)record.requestDate;

                vmList.Add(vm);
            }

            return vmList;
        }

        public int UpdateDetails(string itemcode, int requisitionNo, int? allocateQty, int? fulfilledQty)
        {
            rDAO.UpdateRequisitionDetails(itemcode, requisitionNo, allocateQty, fulfilledQty);
            return 0;
        }

        public void updatestatus(int requisitionNo, int status)
        {
            rDAO.updatestatus(requisitionNo, status);
        }

        public void UpdateDisbursement(string itemCode, int actualQty, string deptCode, int needQty, int count, string staffID)
        {
            int actualTmp = actualQty;
            using (StationeryModel model = new StationeryModel())
            {
                Stationery s = model.Stationeries.Where(x => x.itemCode == itemCode).First();
                s.stockQty = s.stockQty - actualQty;
                model.SaveChanges();
            }
            List<Requisition_Record> list = new List<Requisition_Record>();

            list = GetRecordByItemCode(itemCode).Where(x => x.Department.departmentCode == deptCode && (x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED)).ToList();

            list.Sort();
            for (int i = 0; i < list.Count(); i++)
            {
                var b = list[i].Requisition_Detail.Where(x => x.itemCode == itemCode).First();
                if (b.allocatedQty > 0)
                {
                    if (actualQty - b.allocatedQty >= 0)
                    {
                        actualQty = actualQty - (int)b.allocatedQty;
                        UpdateDetails(itemCode, list[i].requisitionNo, 0, b.allocatedQty + b.fulfilledQty);
                    }
                    else
                    {
                        UpdateDetails(itemCode, list[i].requisitionNo, 0, actualQty + b.fulfilledQty);
                        actualQty = 0;
                    }
                }
            }

            for (int i = 0; i < list.Count(); i++)
            {
                int status = 1;//Collected
                int sum = 0;
                StationeryModel entity = new StationeryModel();

                var NO = list[i].requisitionNo;
                var detailslist = entity.Requisition_Detail.Where(x => x.requisitionNo == NO).ToList();
                foreach (var l in detailslist)
                {
                    sum = sum + (int)l.fulfilledQty;
                    if (l.fulfilledQty == l.qty)
                    {


                    }
                    else
                    {
                        status = 2;//partially fulfilled
                    }
                }
                if (status == 2)
                {
                    if (sum == 0)
                    {
                        status = 3;
                    }
                }
                updatestatus(list[i].requisitionNo, status);
            }

            if (count == 0)
            {
                if (needQty - actualTmp > 0)
                {
                    using (StationeryModel model = new StationeryModel())
                    {
                        Adjustment_Voucher_Record adjustment = new Adjustment_Voucher_Record();
                        adjustment.issueDate = DateTime.Now;
                        adjustment.status = AdjustmentVoucherStatus.PENDING;
                        adjustment.remarks = "NA";
                        adjustment.handlingStaffID = staffID;
                        model.Adjustment_Voucher_Records.Add(adjustment);
                        model.SaveChanges();
                    }
                }
                using (StationeryModel model = new StationeryModel())
                {
                    Transaction_Record tr = new Transaction_Record();
                    tr.clerkID = staffID;
                    tr.date = DateTime.Now;
                    tr.type = TransactionTypes.DISBURSEMENT;
                    model.Transaction_Records.Add(tr);
                    model.SaveChanges();
                }
            }

            using (StationeryModel model = new StationeryModel())
            {
                int max = 0;
                if (needQty - actualTmp > 0)
                {
                    max = 0;
                    foreach(var item in model.Adjustment_Voucher_Records.ToList())
                    {
                        if (item.voucherID > max)
                        {
                            max = item.voucherID;
                        }
                    }
                    Voucher_Detail voucher = new Voucher_Detail();
                    voucher.voucherID = max;
                    voucher.itemCode = itemCode;
                    voucher.adjustedQty = actualTmp - needQty;
                    voucher.remarks = "";
                    model.Voucher_Details.Add(voucher);
                    model.SaveChanges();
                }
                max = 0;
                foreach (var item in model.Transaction_Records.ToList())
                {
                    if (item.transactionNo > max)
                    {
                        max = item.transactionNo;
                    }
                }
                Transaction_Detail detail = new Transaction_Detail();
                detail.transactionNo = max;
                detail.itemCode = itemCode;
                detail.adjustedQty = -actualTmp;
                detail.balanceQty = model.Stationeries.Where(x => x.itemCode == itemCode).First().stockQty;
                detail.remarks = "";
                model.Transaction_Details.Add(detail);
                model.SaveChanges();
            }
        }
         
        public List<Requisition_Record> GetRequestByReqID(string reqid)
        {
            return rDAO.GetRequestByReqID(reqid);
        }
    }

   
}
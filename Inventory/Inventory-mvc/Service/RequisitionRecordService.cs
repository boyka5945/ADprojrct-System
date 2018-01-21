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

        public Requisition_Record GetRequisitionByID(int id)
        {
            return rDAO.FindByRequisitionNo(id);
        }

        public List<Requisition_Detail> GetDetailsByNo(int No=0)
        {
            return rDAO.GetDetailsByNO(No);  
        }

        public void  UpdateRequisition(Requisition_Record rr, string status) 
        {
            rDAO.UpdateRequisition(rr, status);

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

        public bool SubmitNewRequisition(Requisition_Record requisition, string requesterID)
        {
            requisition.requesterID = requesterID;
            requisition.deptCode = userService.FindDeptCodeByID(requesterID);
            requisition.status = RequisitionStatus.PENDING_APPROVAL;
            requisition.requestDate = DateTime.Today;

            if (rDAO.SubmitNewRequisition(requisition))
            {
                // TODO: TEST EMAIL NOTIFICATION
                // send email notification       
                EmailNotification.EmailNotificatioForNewRequisition(requisition.requesterID);

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Requisition_Record> GetRecordsByRequesterID(string requesterID)
        {
            return rDAO.GetRecordsByRequesterID(requesterID);
        }

        public bool ValidateRequisition(Requisition_Record requisition)
        {
            if(requisition.Requisition_Detail.Count == 0)
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

        public List<Disbursement> GetRequisitionByDept(string deptCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetRequisitionByDept(deptCode);
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

    }
}
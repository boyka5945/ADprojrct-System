using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class RequisitionRecordService : IRequisitionRecordService
    {
        IRequisitionRecordDAO rDAO = new RequisitionRecordDAO();

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

        public bool SubmitNewRequisition(Requisition_Record requisition)
        {
            return rDAO.SubmitNewRequisition(requisition);
        }

        public List<Requisition_Record> GetRecordsByRequesterID(string requesterID)
        {
            return rDAO.GetRecordsByRequesterID(requesterID);
        }

        public bool ValidateRequisition(Requisition_Record requisition)
        {
            if(String.IsNullOrEmpty(requisition.requesterID) || requisition.Requisition_Detail.Count == 0)
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

        public bool UpdateDetails(Requisition_Detail requisitionDetail)
        {
            return rDAO.UpdateRequisitionDetails(requisitionDetail);
        }

        public bool ValidateUser(int requisitionNo, string requesterID)
        {
            Requisition_Record record = rDAO.FindByRequisitionNo(requisitionNo);

            return (record.requesterID == requesterID);
        }

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
    }
}
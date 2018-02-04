using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public interface IRequisitionRecordService
    {
        List<Requisition_Record> GetAllRequisition();

        List<Requisition_Record> GetRequisitionRecordByDept(string deptCode);

        List<Requisition_Detail> GetDetailsByNo(int No=0);

        Requisition_Record GetRequisitionByID(int id);

        void UpdateRequisition(Requisition_Record rr, string status, string approveStaffID);

        List<string> GetItemCodeList();

        List<Requisition_Record> GetRecordByItemCode(string itemCode);

        int? FindUnfulfilledQtyBy2Key(string itemCode, int requisitionNo);

        Requisition_Detail FindDetailsBy2Key(string itemCode, int requisitionNo);

        bool SubmitNewRequisition(Requisition_Record requisition, string requesterID);

        List<Disbursement> GetRequisitionByDept(string deptCode);

        List<Requisition_Detail> GetAllRequisitionByDept(string deptCode);

        List<Requisition_Detail> GetPendingRequestByDeptCode(string deptCode);

        List<Disbursement> GetPendingDisbursementByDept(string deptCode);

        List<Requisition_Detail> GetAllPendingDisbursementByDept(string deptCode);

        List<Requisition_Record> GetRecordsByRequesterID(string requesterID);

        bool ValidateRequisition(Requisition_Record requisition);
        void UpdateDetails(string itemcode, int requisitionNo, int? allocateQty);
        List<RetrieveForm> GetRetrieveFormByDateTime(DateTime? time);

        bool DeleteRequisition(int recordNo);
        List<Requisition_Record> GetSortedRecordsByRequesterID(string requesterID, string sortOrder);

        Requisition_Record IsUserAuthorizedForRequisition(int requisitionNo, string requesterID, out string errorMessage);

        int UpdateDetails(string itemcode, int requisitionNo, int? allocateQty, int? fulfilledQty);

        void updatestatus(int requisitionNo, int status);

        bool UpdateRequisitionDetails(List<RequisitionDetailViewModel> vmList, out string errorMessage);

        int DetailsCountOfOneItemcode(string itemcode);

        List<RequisitionDetailViewModel> GetViewModelFromRequisitionRecord(Requisition_Record record);

        bool IsUserValidToSubmitRequisition(string requesterID);


        void UpdateDisbursement(string itemCode, int actualQty, string deptCode, int needQty, int count, string staffID);

        List<Requisition_Record> GetRequestByReqID(string reqid);

        void UpdateDetailsAndroid(string itemcode, int requisitionNo, int? allocateQty);

        // TODO - REMOVE THIS METHOD
        bool GenerateRandomRequisition(Requisition_Record requisition, string requesterID, DateTime date);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.DAO
{
    public interface IRequisitionRecordDAO
    {
        List<Requisition_Record> GetAllRequisition();

        List<Requisition_Record> GetRequisitionRecordByDept(string deptCode);

        Requisition_Record FindByRequisitionNo(int requisitionNo);

        Boolean AddNewRequisition(Requisition_Record purchase_order_record);

        int UpdateRequisition(Requisition_Record requisition_Record, string status, string approveStaffID);

        Boolean DeleteRequisition(int requisitionNo);

        List<Requisition_Record> GetRecordByItemCode(string itemCode);

        List<string> GetDetailsByGroup();

        int UpdateRequisitionDetails(string itemcode, int requisitionNo, int? allocateQty);

        int UpdateRequisitionDetails(string itemcode, int requisitionNo, int? allocateQty, int? fulfilledQty);

        void updatestatus(int requisitionNo, int status);

        List<Requisition_Detail> GetDetailsByNO(int No=0);

        int? FindUnfulfilledQtyBy2Key(string itemcode, int requisionNo);

        Requisition_Detail FindDetailsBy2Key(string itemCode, int requisitionNo);

        int DetailsCountOfOneItemcode(string itemCode);

        List<Disbursement> GetRequisitionByDept(string deptCode);

        List<Requisition_Detail> GetAllRequisitionByDept(string deptCode);
              
        List<Disbursement> GetPendingDisbursementByDept(string deptCode);

        List<Requisition_Detail> GetAllPendingDisbursementByDept(string deptCode);

        bool SubmitNewRequisition(Requisition_Record requisition);

        List<Requisition_Record> GetRecordsByRequesterID(string requesterID);

        List<RetrieveForm> GetRetrieveFormByDateTime(DateTime? time);

        bool UpdateRequisitionDetails(Requisition_Detail requisitionDetail);
        List<Requisition_Record> GetSortedRecordsByRequesterID(string requesterID, string sortOrder);
        int UpdateRequisitionDetailsAndroid(string itemcode, int requisitionNo, int? allocateQty);

        List<Requisition_Record> GetRequestByReqID(string id);
    }
}

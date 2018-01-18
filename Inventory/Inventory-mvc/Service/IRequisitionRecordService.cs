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

        List<Requisition_Detail> GetDetailsByNo(int No=0);

        Requisition_Record GetRequisitionByID(int id);

        void UpdateRequisition(Requisition_Record rr, string status);

        List<string> GetItemCodeList();

        List<Requisition_Record> GetRecordByItemCode(string itemCode);

        int? FindUnfulfilledQtyBy2Key(string itemCode, int requisitionNo);

        Requisition_Detail FindDetailsBy2Key(string itemCode, int requisitionNo);

        

        bool SubmitNewRequisition(Requisition_Record requisition);
        List<Disbursement> GetRequisitionByDept(string deptCode);

        List<Requisition_Record> GetRecordsByRequesterID(string requesterID);

        bool ValidateRequisition(Requisition_Record requisition);
        void UpdateDetails(string itemcode, int requisitionNo, int? allocateQty);

        bool DeleteRequisition(int recordNo);

        bool UpdateDetails(Requisition_Detail requisitionDetail);

        bool ValidateUser(int requisitionNo, string requesterID);
    }
}
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

        List<Requisition_Details> GetDetailsByNo(int No=0);

        Requisition_Record GetRequisitionByID(int id);

        void UpdateRequisition(Requisition_Record rr, string status);

        List<string> GetItemCodeList();

        List<Requisition_Record> GetRecordByItemCode(string itemCode);

        int FindUnfulfilledQtyBy2Key(string itemCode, int requisitionNo);

        Requisition_Details FindDetailsBy2Key(string itemCode, int requisitionNo);

        List<Disbursement> GetRequisitionByDept(string deptCode);

        void UpdateDetails(string itemcode, int requisitionNo, int allocateQty);
    }
}
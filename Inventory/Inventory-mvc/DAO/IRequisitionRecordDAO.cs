using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IRequisitionRecordDAO
    {
        List<Requisition_Record> GetAllRequisition();

        Requisition_Record FindByRequisitionNo(int requisitionNo);

        Boolean AddNewRequisition(Requisition_Record purchase_order_record);

        int UpdateRequisition(Requisition_Record purchase_order_record, string status);

        Boolean DeleteRequisition(int requisitionNo);

        List<Requisition_Record> GetRecordByItemCode(string itemCode);

        List<string> GetDetailsByGroup();

        int UpdateRequisitionDetails(string itemcode, int requisitionNo, int allocateQty);

        List<Requisition_Details> GetDetailsByNO(int No=0);

        int FindUnfulfilledQtyBy2Key(string itemcode, int requisionNo);

        Requisition_Details FindDetailsBy2Key(string itemCode, int requisitionNo);
    }
}
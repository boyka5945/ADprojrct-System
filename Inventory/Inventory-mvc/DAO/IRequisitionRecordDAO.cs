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

        int UpdateRequisition(Requisition_Record purchase_order_record);

        Boolean DeleteRequisition(int requisitionNo);
    }
}
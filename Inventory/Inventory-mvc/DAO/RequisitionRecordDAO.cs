using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class RequisitionRecordDAO : IRequisitionRecordDAO
    {

        public List<Requisition_Record> GetAllRequisition()
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Record.ToList();
        }

        public Requisition_Record FindByRequisitionNo(int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Record.Where(x => x.requisitionNo == requisitionNo).First();
        }

        public Boolean AddNewRequisition(Requisition_Record requisitionRecord)
        {

            try
            {
                StationeryModel entity = new StationeryModel();
                Requisition_Record requisition = new Requisition_Record();
                requisition.requisitionNo = requisitionRecord.requisitionNo;
                requisition.requestDate = requisitionRecord.requestDate;
                requisition.approveDate = requisitionRecord.approveDate;
                requisition.approvingStaffID = requisitionRecord.approvingStaffID;
                requisition.deptCode = requisitionRecord.deptCode;
                requisition.status = requisitionRecord.status;
                requisition.requesterID = requisitionRecord.requesterID;
                entity.Requisition_Record.Add(requisition);
                entity.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateRequisition(Requisition_Record purchase_order_record)
        {
            StationeryModel entity = new StationeryModel();
            return 10;
        }

        public Boolean DeleteRequisition(int requisitionNo)
        {
            return true;
        }
    }
}
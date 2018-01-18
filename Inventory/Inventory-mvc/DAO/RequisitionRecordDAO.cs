using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

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

            using (StationeryModel entity = new StationeryModel()) {
                try
                {
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
            }
            return true;
        }

        public int UpdateRequisition(Requisition_Record requisition_Record, string status)
        {
            Requisition_Record rr = new Requisition_Record();
            StationeryModel entity = new StationeryModel();
            rr = entity.Requisition_Record.Where(x => x.requisitionNo == requisition_Record.requisitionNo).First();
            rr.status = status;
            entity.SaveChanges();
            return 0;
        }

        public int UpdateRequisitionDetails(string itemcode, int requisitionNo, int allocateQty)
        {
            StationeryModel entity = new StationeryModel();
            Requisition_Details rd = new Requisition_Details();
            rd = entity.Requisition_Details.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
            rd.allocatedQty = allocateQty;
            entity.SaveChanges();
            return 0;
        }

        public Boolean DeleteRequisition(int requisitionNo)
        {
            return true;
        }

        public List<Requisition_Details> GetDetailsByNO(int No=0)
        {
            StationeryModel entity = new StationeryModel();
            if (No == 0)
            {
                return entity.Requisition_Details.ToList();
            }
            else
            {
                
                return entity.Requisition_Record.Where(x => x.requisitionNo == No).First().Requisition_Details.ToList();
            }
        }

        public  List<string> GetDetailsByGroup()
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Details> list = new List<Requisition_Details>();
            List<string> list2 = new List<string>();
          
            var groups = entity.Requisition_Details.Select(x => new
            {
                itemcode = x.itemCode
            }).ToList();
            var newgroups = groups.Distinct().ToList();
            foreach (var group in newgroups)
            {
                list2.Add(group.itemcode);
            }
            return list2;
        }

        public List<Requisition_Record> GetRecordByItemCode(string itemCode)
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> returnList = new List<Requisition_Record>();
            List<Requisition_Details> list = entity.Requisition_Details.Where(x => x.itemCode == itemCode).ToList();
            foreach(var l in list)
            {
                returnList.Add(FindByRequisitionNo(l.requisitionNo));
            }
            return returnList;
        }

        public int FindUnfulfilledQtyBy2Key(string itemcode, int requisionNo)
        {
            StationeryModel entity = new StationeryModel();
            Requisition_Details rd = entity.Requisition_Details.Where(x => x.itemCode == itemcode && x.requisitionNo == requisionNo).First();
            if (rd.fulfilledQty != null)
            {
                return (int)rd.qty - (int)rd.fulfilledQty;
            }
            else
            {
                return rd.qty;
            }
        }

        public Requisition_Details FindDetailsBy2Key(string itemcode, int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Details.Where(x=>x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
            
        }

        public List<Disbursement> GetRequisitionByDept(string deptCode)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();
            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> list = entity.Requisition_Record.Where(x => x.deptCode == deptCode).ToList();
            List<string> itemCodes = new List<string>();
            List<int> Qty = new List<int>();
            foreach (var item in list)
            {
                List<Requisition_Details> rd = item.Requisition_Details.ToList();
                foreach (var a in rd)
                {
                    if (!itemCodes.Contains(a.itemCode))
                    {
                        itemCodes.Add(a.itemCode);
                    }
                }
            }
            for (int i = 0; i < itemCodes.Count; i++)
            {
                Qty.Add(0);
            }
            for (int i = 0; i<itemCodes.Count; i++)
            {
                foreach (var b in list)
                {
                    if ((b.Requisition_Details.Where(x => x.itemCode == itemCodes[i]).Count()) > 0)
                    {
                        Qty[i] = Qty[i] + (int)b.Requisition_Details.Where(x => x.itemCode == itemCodes[i]).First().allocatedQty;
                    }
                }
            }
            for (int i = 0; i < itemCodes.Count; i++)
            {
                Disbursement disbursement = new Disbursement();
                disbursement.itemDescription = itemCodes[i];
                disbursement.quantity = Qty[i];
                disbursementList.Add(disbursement);
            }
            return disbursementList;

        }
    }
}
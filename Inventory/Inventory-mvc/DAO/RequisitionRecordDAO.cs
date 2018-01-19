using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class RequisitionRecordDAO : IRequisitionRecordDAO
    {

        public List<Requisition_Record> GetAllRequisition()
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Records.ToList();
        }

        public Requisition_Record FindByRequisitionNo(int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Records.Where(x => x.requisitionNo == requisitionNo).FirstOrDefault();
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
                    entity.Requisition_Records.Add(requisition);
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
            rr = entity.Requisition_Records.Where(x => x.requisitionNo == requisition_Record.requisitionNo).First();
            rr.status = status;
            entity.SaveChanges();
            return 0;
        }

        public int UpdateRequisitionDetails(string itemcode, int requisitionNo, int? allocateQty)
        {
            StationeryModel entity = new StationeryModel();
            Requisition_Detail rd = new Requisition_Detail();
            rd = entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
            rd.allocatedQty = allocateQty;
            entity.SaveChanges();
            return 0;
        }

        public Boolean DeleteRequisition(int requisitionNo)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    Requisition_Record record = (from r in context.Requisition_Records
                                                 where r.requisitionNo == requisitionNo
                                                 select r).FirstOrDefault();

                    context.Requisition_Detail.RemoveRange(record.Requisition_Detail);                    
                    context.Requisition_Records.Remove(record);
                    context.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public List<Requisition_Detail> GetDetailsByNO(int No=0)
        {
            StationeryModel entity = new StationeryModel();
            if (No == 0)
            {
                return entity.Requisition_Detail.ToList();
            }
            else
            {
                
                return entity.Requisition_Records.Where(x => x.requisitionNo == No).First().Requisition_Detail.ToList();
            }
        }

        public  List<string> GetDetailsByGroup()
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Detail> list = new List<Requisition_Detail>();
            List<string> list2 = new List<string>();
          
            var groups = entity.Requisition_Detail.Select(x => new
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
            List<Requisition_Detail> list = entity.Requisition_Detail.Where(x => x.itemCode == itemCode).ToList();
            foreach(var l in list)
            {
                returnList.Add(FindByRequisitionNo(l.requisitionNo));
            }
            return returnList;
        }

        public int? FindUnfulfilledQtyBy2Key(string itemcode, int requisionNo)
        {
            StationeryModel entity = new StationeryModel();
            Requisition_Detail rd = entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisionNo).First();
            if (rd.fulfilledQty != null)
            {
                return rd.qty - rd.fulfilledQty;
            }
            else
            {
                return rd.qty;
            }
        }

        public Requisition_Detail FindDetailsBy2Key(string itemcode, int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Detail.Where(x=>x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
            
        }

        public List<Disbursement> GetRequisitionByDept(string deptCode)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();
            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> list = entity.Requisition_Records.Where(x => x.deptCode == deptCode).ToList();
            List<string> itemCodes = new List<string>();
            List<int?> Qty = new List<int?>();
            foreach (var item in list)
            {
                List<Requisition_Detail> rd = item.Requisition_Detail.ToList();
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
                Qty.Add((int?)0);
            }
            for (int i = 0; i<itemCodes.Count; i++)
            {
                foreach (var b in list)
                {
                    if ((b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).Count()) > 0)
                    {
                        Qty[i] = Qty[i] + b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).First().allocatedQty;
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

        public bool SubmitNewRequisition(Requisition_Record requisition)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    context.Requisition_Records.Add(requisition);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public List<Requisition_Record> GetRecordsByRequesterID(string requesterID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Requisition_Records
                        where r.requesterID == requesterID
                        select r).Include(r => r.Requisition_Detail).ToList();
            }
        }

        public List<RetrieveForm> GetRetrieveFormByDateTime(DateTime? time)
        {

            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> rr = entity.Requisition_Records.Where(x => x.approveDate < time).ToList();
            List<RetrieveForm> retrieveList = new List<RetrieveForm>();
            List<string> ItemCodes = new List<string>();
            List<int?> Qty = new List<int?>();
            foreach (var item in rr)
            {
                var list = item.Requisition_Detail.ToList();
                foreach (var l in list)
                {
                    if (!ItemCodes.Contains(l.itemCode))
                    {
                        ItemCodes.Add(l.itemCode);
                    }
                }
            }
            for (int i = 0; i < ItemCodes.Count; i++)
            {
                Qty.Add((int?)0);
            }
            for (int i = 0; i < ItemCodes.Count; i++)
            {
                foreach (var b in rr)
                {
                    if ((b.Requisition_Detail.Where(x => x.itemCode == ItemCodes[i]).Count()) > 0)
                    {
                        Qty[i] = Qty[i] + b.Requisition_Detail.Where(x => x.itemCode == ItemCodes[i]).First().allocatedQty;
                    }
                }
            }
            for (int i = 0; i < ItemCodes.Count; i++)
            {
                RetrieveForm rf = new RetrieveForm();
                rf.description = ItemCodes[i];
                rf.Qty = Qty[i];
                retrieveList.Add(rf);
            }
            return retrieveList;
        }
        public bool UpdateRequisitionDetails(Requisition_Detail requisitionDetail)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    Requisition_Detail rd = (from r in context.Requisition_Detail
                                             where r.requisitionNo == requisitionDetail.requisitionNo
                                             & r.itemCode == requisitionDetail.itemCode
                                             select r).FirstOrDefault();

                    rd.qty = requisitionDetail.qty;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }
    }
}
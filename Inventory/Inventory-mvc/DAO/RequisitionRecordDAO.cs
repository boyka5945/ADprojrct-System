using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
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

        public List<Requisition_Record> GetRequisitionRecordByDept(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Records.Where(x => x.deptCode == deptCode && x.status== "Pending Approval").ToList();
        }

        public Requisition_Record FindByRequisitionNo(int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Records.Where(x => x.requisitionNo == requisitionNo).FirstOrDefault();
        }

        public Boolean AddNewRequisition(Requisition_Record requisitionRecord)
        {

            using (StationeryModel entity = new StationeryModel())
            {
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
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public int UpdateRequisition(Requisition_Record requisition_Record, string status, string approveStaffID)
        {
            Requisition_Record rr = new Requisition_Record();
            StationeryModel entity = new StationeryModel();
            rr = entity.Requisition_Records.Where(x => x.requisitionNo == requisition_Record.requisitionNo).First();
            rr.status = status;

            if (approveStaffID == "")
            {
                rr.approvingStaffID = null;
                rr.approveDate = null;
            }
            else
            {
                rr.approvingStaffID = approveStaffID;
                rr.approveDate = DateTime.Now;
            }
            entity.SaveChanges();
            return 0;
        }

        public int UpdateRequisitionDetails(string itemcode, int requisitionNo, int? allocateQty)
        {
            try
            {
                StationeryModel entity = new StationeryModel();
                Requisition_Detail rd = new Requisition_Detail();
                rd = entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
                rd.allocatedQty = allocateQty;
                entity.SaveChanges();
                return 0;
            }
            catch (Exception e){
                e.Message.ToString();
                return 1;
            }
        }
        //for android: different cos of execution of rd.allocateQty += allocateQty
        public int UpdateRequisitionDetailsAndroid(string itemcode, int requisitionNo, int? allocateQty)
        {
            try
            {
                StationeryModel entity = new StationeryModel();
                Requisition_Detail rd = new Requisition_Detail();
                rd = entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
                //start of code added by alex//
                //deduct retrieve qty after doing allocation
                List<RetrieveForm> retrievalList = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
                int index = retrievalList.FindIndex(x => x.ItemCode == itemcode);
                retrievalList[index].retrieveQty -= allocateQty;


                HttpContext.Current.Application["retrieveForm"] = retrievalList;

                //end of code//
                rd.allocatedQty += allocateQty;
                entity.SaveChanges();
                return 0;
            }
            catch
            {
                return 0;
            }


        }

        public int UpdateRequisitionDetails(string itemcode, int requisitionNo, int? allocateQty, int? fulfilledQty)
        {
            StationeryModel entity = new StationeryModel();
            Requisition_Detail rd = new Requisition_Detail();
            rd = entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();
            rd.allocatedQty = allocateQty;
            rd.fulfilledQty = fulfilledQty;
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

        public List<Requisition_Detail> GetDetailsByNO(int No = 0)
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

        public List<string> GetDetailsByGroup()
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
            foreach (var l in list)
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
            return entity.Requisition_Detail.Where(x => x.itemCode == itemcode && x.requisitionNo == requisitionNo).First();

        }
        

        public List<Disbursement> GetRequisitionByDept(string deptCode)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();
            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> list;
            if (deptCode != "")
            {
                list = entity.Requisition_Records.Where(x => x.deptCode == deptCode && (x.status == "Approved and Processing" || x.status == "Partially fulfilled")).ToList();
            }
            else
            {
                list = entity.Requisition_Records.Where(x => x.status == "Approved and Processing" || x.status == "Partially fulfilled").ToList();
            }
            List<string> itemCodes = new List<string>();
            List<int?> Qty = new List<int?>();
            foreach (var item in list)
            {
                List<Requisition_Detail> rd = item.Requisition_Detail.Where(x => x.allocatedQty > 0).ToList();
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
            for (int i = 0; i < itemCodes.Count; i++)
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
                disbursement.itemCode = itemCodes[i];
                var A = itemCodes[i];
                disbursement.itemDescription = entity.Stationeries.Where(x => x.itemCode == A).First().description;
                disbursement.quantity = Qty[i];
                disbursement.actualQty = 0;
                disbursement.departmentCode = deptCode;
                disbursementList.Add(disbursement);
            }
            return disbursementList;


        }

        public List<Requisition_Detail> GetAllRequisitionByDept(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Detail> requisitionList = new List<Requisition_Detail>();
            List<Requisition_Record> list = entity.Requisition_Records.Where(x => x.deptCode == deptCode).ToList();
            foreach(var request in list)
            {
                List<Requisition_Detail> rd = request.Requisition_Detail.Where(x => x.requisitionNo == request.requisitionNo&&x.allocatedQty>0).ToList();
              
                foreach(var a in rd)
                {
                    requisitionList.Add(a);
                }
            }
           

            return requisitionList;
        }

        public List<Requisition_Detail> GetPendingRequestByDeptCode(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Detail> requisitionList = new List<Requisition_Detail>();
            List<Requisition_Record> list = entity.Requisition_Records.Where(x => x.deptCode == deptCode).ToList();
            foreach (var request in list)
            {
                List<Requisition_Detail> rd = request.Requisition_Detail.Where(x => x.requisitionNo == request.requisitionNo && request.status== "Pending Approval").ToList();

                foreach (var a in rd)
                {
                    requisitionList.Add(a);
                }
            }


            return requisitionList;
        }

        public List<Disbursement> GetPendingDisbursementByDept(string deptCode)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();
            StationeryModel entity = new StationeryModel();
            List<Requisition_Record> list = entity.Requisition_Records.Where(x => x.deptCode == deptCode &&( x.status == "Approved and Processing" || x.status == "Partially fulfilled")).ToList();
            List<string> itemCodes = new List<string>();
            List<int?> Qty = new List<int?>();
            foreach (var item in list)
            {
                List<Requisition_Detail> rd = item.Requisition_Detail.Where(x => x.qty-x.fulfilledQty >x.allocatedQty).ToList();
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
            for (int i = 0; i < itemCodes.Count; i++)
            {
                foreach (var b in list)
                {
                    if ((b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).Count()) > 0)
                    {
                        Qty[i] = Qty[i] + (b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).First().qty)- (b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).First().fulfilledQty)- (b.Requisition_Detail.Where(x => x.itemCode == itemCodes[i]).First().allocatedQty);
                    }
                }
            }
            for (int i = 0; i < itemCodes.Count; i++)
            {
                Disbursement disbursement = new Disbursement();
                disbursement.itemCode = itemCodes[i];
                var A = itemCodes[i];
                disbursement.itemDescription = entity.Stationeries.Where(x => x.itemCode == A).First().description;
                disbursement.quantity = Qty[i];
                
                disbursement.departmentCode = deptCode;
                disbursementList.Add(disbursement);
            }
            return disbursementList;


        }

        public List<Requisition_Detail> GetAllPendingDisbursementByDept(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            List<Requisition_Detail> requisitionList = new List<Requisition_Detail>();
            List<Requisition_Record> list = entity.Requisition_Records.Where(x => x.deptCode == deptCode && (x.status == "Approved and Processing" || x.status == "Partially fulfilled")).ToList();
            foreach (var request in list)
            {
                List<Requisition_Detail> rd = request.Requisition_Detail.Where(x => x.qty - x.fulfilledQty > x.allocatedQty ).ToList();

                foreach (var a in rd)
                {
                    requisitionList.Add(a);
                }
            }


            return requisitionList;
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
                    throw new Exception(e.Message);
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
            List<Requisition_Record> rr = entity.Requisition_Records.Where(x => x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED).ToList();
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
                    if ((b.Requisition_Detail.Where(x => x.itemCode == ItemCodes[i]).Count()) > 0 && (b.status == RequisitionStatus.APPROVED_PROCESSING || b.status == RequisitionStatus.PARTIALLY_FULFILLED))
                    {
                        Qty[i] = Qty[i] + b.Requisition_Detail.Where(x => x.itemCode == ItemCodes[i]).First().qty - b.Requisition_Detail.Where(x => x.itemCode == ItemCodes[i]).First().fulfilledQty;
                    }
                }
            }
            for (int i = 0; i < ItemCodes.Count; i++)
            {
                RetrieveForm rf = new RetrieveForm();
                rf.ItemCode = ItemCodes[i];
                rf.description = entity.Stationeries.Where(x => x.itemCode == rf.ItemCode).First().description;
                rf.Qty = Qty[i];
                rf.retrieveQty = 0;
                rf.StockQty = entity.Stationeries.Where(x => x.itemCode == rf.ItemCode).First().stockQty;
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
                    rd.Requisition_Record.requestDate = DateTime.Today;

                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }

        public List<Requisition_Record> GetSortedRecordsByRequesterID(string requesterID, string sortOrder)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var records = from r in context.Requisition_Records where r.requesterID == requesterID select r;

                switch (sortOrder)
                {
                    case "number_desc":
                        records = records.OrderByDescending(r => r.requisitionNo);
                        break;
                    case "Requisition Form Number":
                        records = records.OrderBy(r => r.requisitionNo);
                        break;
                    case "Request Date":
                        records = records.OrderBy(r => r.requestDate);
                        break;
                    case "date_desc":
                        records = records.OrderByDescending(r => r.requestDate);
                        break;
                    case "Status":
                        records = records.OrderBy(r => r.status);
                        break;
                    case "status_desc":
                        records = records.OrderByDescending(r => r.status);
                        break;
                    default:
                        records = records.OrderByDescending(r => r.requestDate);
                        break;
                }

                return records.ToList();
            }

        }

        public int DetailsCountOfOneItemcode(string itemCode)
        {
            int count = 0;
            StationeryModel entity = new StationeryModel();
            List<Requisition_Detail> l = entity.Requisition_Detail.Where(x => x.itemCode == itemCode).ToList();
            foreach(var item in l)
            {
                if(item.Requisition_Record.status == RequisitionStatus.APPROVED_PROCESSING || item.Requisition_Record.status == RequisitionStatus.PARTIALLY_FULFILLED)
                {
                    count++;
                }
            }
            return count;
        }

        public void updatestatus(int requisitionNo, int status)
        {
            using (StationeryModel entity = new StationeryModel()){
                var record = entity.Requisition_Records.Where(x => x.requisitionNo == requisitionNo).First();
                if (status == 1)
                {
                    record.status = RequisitionStatus.COLLECTED;
                }
                else if (status == 2)
                {
                    record.status = RequisitionStatus.PARTIALLY_FULFILLED;
                }
                else
                {
                    record.status = RequisitionStatus.APPROVED_PROCESSING;
                }
                entity.SaveChanges();
            }
        }

        public void updatestatus(int requisitionNo)
        {
            throw new NotImplementedException();
        }

        public List<Requisition_Record> GetRequestByReqID(string id)
        {
            StationeryModel entity = new StationeryModel();
            return (from req in entity.Requisition_Records where req.requesterID == id select req).ToList();
        }
    }
}
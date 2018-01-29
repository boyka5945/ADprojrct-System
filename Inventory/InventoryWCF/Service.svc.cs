using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System.Web;
using Inventory_mvc.Utilities;
using Inventory_mvc.Function;

namespace InventoryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService
    {
        IUserService userService = new UserService();
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionRecordService = new RequisitionRecordService();
        DepartmentService departmentService = new DepartmentService();
        ICollectionPointService cpService = new CollectionPointService();




        public Boolean ValidateUser(WCFUser User)
        {
            //return BusinessLogic.validateUser(userid, password);

            try
            {
                User user = userService.FindByUserID(User.UserID);
                if (user != null)
                {
                    if (User.PassWord == Encrypt.DecryptMethod(user.password))
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                // non existing userID
                return false;
            }           
        }

        public WCFUser GetUser(string userID, string password)
        {

            try
            {
                User user = userService.FindByUserID(userID);
                if (user.password == password)
                {
                    WCFUser wcfUser = WCFModelConvertUtility.ConvertToWCFUser(user);
                    return wcfUser;
                }
                else
                {
                    WCFUser invalid = new WCFUser();
                    return invalid;
                }
            }
            catch (Exception e)
            {
                WCFUser invalid = new WCFUser();
                return invalid;
            }
        }


        public Boolean ChangePassword(string userid, string currentpassword, string newpassword)
        {
            WCFUser u = new WCFUser();
            u.UserID = userid;
            u.PassWord = currentpassword;
            
            // TODO : IMPLEMENT METHOD
            //return BusinessLogic.changePassWord(userid, currentpassword, newpassword);
            try
            {
                if(ValidateUser(u))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // invalid user
                    return false;
                }
            }
            catch (Exception e)
            {
                // error
                return false;
            }

        }


        //public List<string> GetAllItemCode()
        //{
        //    
        //    //return BusinessLogic.getAllItemCode();
        //    try
        //    {
                
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}

        public List<WCFRequisitionDetail> GetRequisitionDetailsByItemCode(string itemCode)
        {
            // TODO : IMPLEMENT METHOD

            throw new NotImplementedException();

            //List<WCFRequisitionDetail> list = new List<WCFRequisitionDetail>();
            //var rr = BusinessLogic.getRequisitionDetailsByItemCode(itemCode);
            //foreach (var item in rr)
            //{
            //    WCFRequisitionDetail r = new WCFRequisitionDetail();
            //    r.RequisitonNo = item.requisitionNo;
            //    r.RetrievedDate = item.retrievedDate;
            //    r.ItemCode = item.itemCode;
            //    r.NextCollectionDate = item.nextCollectionDate;
            //    r.Qty = (int) item.qty;
            //    r.ClerkID = item.clerkID;
            //    r.AllocateQty = (int)item.allocatedQty;
            //    r.FulfilledQty = (int)item.fulfilledQty;
            //    r.Remarks = item.remarks;
            //    list.Add(r);
            //}
            //return list;
        }

        public WCFRequisitionDetail GetRequisitionDetailsBy2Keys(string itemCode, string requisitionNO)
        {
            try
            {
                Requisition_Detail detail = requisitionRecordService.FindDetailsBy2Key(itemCode, Convert.ToInt32(requisitionNO));
                return WCFModelConvertUtility.ConvertToWCFRequisitionDetail(detail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WCFStationery> GetAllStationeries()
        {
            try
            {
                List<Stationery> stationeries = stationeryService.GetAllStationery();
                return WCFModelConvertUtility.ConvertToWCFStationery(stationeries);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public WCFStationery GetStationery(string itemCode)
        {
            try
            {
                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
                return WCFModelConvertUtility.ConvertToWCFStationery(s);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WCFStationery> GetStationeryByCriteria(string categoryName, string searchString)
        {
            try
            {
                string categoryID = "-1";

                if(categoryName != "All")
                {
                    List<Category> categories = stationeryService.GetAllCategory();
                    categoryID = categories.Find(x => x.categoryName == categoryName).categoryID.ToString();
                }

                List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(searchString, categoryID);
                return WCFModelConvertUtility.ConvertToWCFStationery(stationeries);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WCFStationery> GetStationeryByCategory(string categoryName)
        {
            try
            {
                string categoryID = "-1";

                if (categoryName != "All")
                {
                    List<Category> categories = stationeryService.GetAllCategory();
                    categoryID = categories.Find(x => x.categoryName == categoryName).categoryID.ToString();
                }

                List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(null, categoryID);
                return WCFModelConvertUtility.ConvertToWCFStationery(stationeries);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


        public List<WCFCategory> GetAllCategories()
        {
            try
            {
                List<Category> categories = stationeryService.GetAllCategory();
                return WCFModelConvertUtility.ConvertToWCFCategories(categories);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WCFRequisitionRecord> GetAllRequisitionRecords()
        {
            List<Requisition_Record> requisitionRecords = requisitionRecordService.GetAllRequisition();
            return WCFModelConvertUtility.ConvertToWCFRequisitionRecord(requisitionRecords);
        }

        public List<WCFRequisitionRecord> GetRequsitionRecordByDept(string deptCode)
        {
            List<Requisition_Record> reqByDept = requisitionRecordService.GetRequisitionRecordByDept(deptCode);
            return WCFModelConvertUtility.ConvertToWCFRequisitionRecord(reqByDept);
        }

        

        public bool AddNewRequest(string requesterID, WCFRequisitionDetail[] newRequisition)
        {
            Requisition_Record newRecord = new Requisition_Record();
            newRecord.Requisition_Detail = new List<Requisition_Detail>();
            //newRecord.Requisition_Detail.Add(WCFModelConvertUtility.ConvertFromWCFRequisitionDetail(newRequisition));

            foreach (var wcf_detail in newRequisition)
            {
                newRecord.Requisition_Detail.Add(WCFModelConvertUtility.ConvertFromWCFRequisitionDetail(wcf_detail));
            }

            try
            {
                requisitionRecordService.SubmitNewRequisition(newRecord, requesterID);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WCFRequisitionDetail> GetPendingRequestByDept(string deptCode)
        {
            List<Requisition_Detail> reqDetail = requisitionRecordService.GetPendingRequestByDeptCode(deptCode);
            return WCFModelConvertUtility.ConvertToWCFRequestionDetails(reqDetail);
        }

        public List<WCFRequisitionDetail> GetDetailsByReqNo(string reqNo)
        {
            int no = Int32.Parse(reqNo);
            List<Requisition_Detail> reqDetail = requisitionRecordService.GetDetailsByNo(no);
            return WCFModelConvertUtility.ConvertToWCFRequestionDetails(reqDetail);
        }
        //public Boolean updateRequisitionDetails(int requisitionNo, string ItemCode, int allocateQty)
        //{
        //    return BusinessLogic.updateRequisitionDetails(requisitionNo, ItemCode, allocateQty);
        //}

        public List<WCFRetrievalForm> getRetrievalList()
        {
            StationeryModel entity = new StationeryModel();
            List<RetrieveForm> list = new List<RetrieveForm>();
             DateTime date = DateTime.Now;
            if (HttpContext.Current.Application["retrieveForm"] != null)
            {
                list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
            }
            else
            {
                list = requisitionRecordService.GetRetrieveFormByDateTime(date); //newly generated list
                HttpContext.Current.Application["retrieveForm"] = list;
            }

            //generate list of requisition records for allocation at the same time
            List<Requisition_Record> rr = entity.Requisition_Records.Where(x => x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED).ToList();
            HttpContext.Current.Application["requisitionRecordList_allocation"] = rr;

            return WCFModelConvertUtility.ConvertToWCFRetrievalList(list);
            
        }

        public WCFRetrievalForm GetRetrievalForm(string itemCode)
        {
            List<RetrieveForm> list = new List<RetrieveForm>();

            if (HttpContext.Current.Application["retrieveForm"] != null)
            {
                list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
            }
            else
            {
                //can be assumed no items have been retrieved yet since there is no retrieval list generated
            }

            WCFRetrievalForm item = WCFModelConvertUtility.ConvertToWCFRetrieval(list.Where(x => x.ItemCode == itemCode).First());

            return item;

        }

        public bool UpdateRetrieval(WCFRetrievalForm wcfr)
        {
            List<RetrieveForm> list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
            //RetrieveForm rf = list.Where(x => x.description == description).First();
            //rf.retrieveQty = Int32.Parse(qty);
            var index = list.FindIndex(x => x.description == wcfr.Description);
            list[index].retrieveQty = wcfr.QtyRetrieved;

            HttpContext.Current.Application["retrieveForm"] = list;
            return true;


        }


        public List<WCFDepartment> GetAllDepartments()
        {
            List<Department> departmentList = departmentService.GetAllDepartment();
            return WCFModelConvertUtility.ConvertToWCFDepartments(departmentList);


        }

        public List<WCFCollectionPoint> GetAllCollectionPoints()
        {
            List<Collection_Point> cpList = cpService.GetAllCollectionPoints2();
            return WCFModelConvertUtility.convertToWCFCollectionPoints(cpList);


        }

        public List<WCFDisbursement> GetDisbursementByDept(string deptCode)
        {
            List<Disbursement> disbursement = requisitionRecordService.GetRequisitionByDept(deptCode);
            return WCFModelConvertUtility.ConvertToWCFDisbursement(disbursement, deptCode);
        }

        //allocation list should match the retrieval list
        public List<WCFRequisitionDetail> GetAllRequisitionDetailsforAllocation()
        {
            List<WCFRequisitionDetail> allocationList = new List<WCFRequisitionDetail>();
            if(HttpContext.Current.Application["requisitionRecordList_allocation"] == null)
            {
                return allocationList = null; //empty and need to return some error msg
            }

            List<Requisition_Record> records = (List<Requisition_Record>)HttpContext.Current.Application["requisitionRecordList_allocation"];

            foreach(Requisition_Record rr in records)
            {
                List<Requisition_Detail> temp =  rr.Requisition_Detail.ToList();
                List<WCFRequisitionDetail> wcftemp = WCFModelConvertUtility.ConvertToWCFRequestionDetails(temp);
                allocationList.AddRange(wcftemp);
            }

            return allocationList;
        }

        public void UpdateReqDetail(string NO, string itemCode, string quantity)
        {
            int requisitionNo = Convert.ToInt32(NO);
            int qty = Convert.ToInt32(quantity);
            using (StationeryModel entity = new StationeryModel())
            {
                entity.Requisition_Detail.Where(x => x.itemCode == itemCode && x.requisitionNo == requisitionNo).First().qty = (int)qty;
                entity.SaveChanges();
            }
            //Requisition_Detail requisitionDetail = new Requisition_Detail
            //{
            //    requisitionNo = reqDetail.RequisitionNo,
            //    itemCode = reqDetail.ItemCode,
            //    remarks = reqDetail.Remarks,
            //    qty = reqDetail.Qty,
            //    fulfilledQty = reqDetail.FulfilledQty,
            //    clerkID = reqDetail.ClerkID,
            //    retrievedDate = reqDetail.RetrievedDate,
            //    allocatedQty = reqDetail.AllocateQty,
            //    nextCollectionDate = reqDetail.NextCollectionDate
            //};
            //WCFModelConvertUtility.ConvertToWCFRequestionDetails(UpdateReqDetail(requisitionDetail));
            //requisitionRecordService.up UpdateReqDetail(WCFModelConvertUtility.ConvertToWCFRequestionDetails(requisitionDetail);
        }

        public List<WCFDisbursement> GetPendingItemsToBeProcessedByDepartmentByItems(string deptCode)
        {

            List<Disbursement> pendingItemsByItem = requisitionRecordService.GetPendingDisbursementByDept(deptCode);
            return WCFModelConvertUtility.ConvertToWCFDisbursement(pendingItemsByItem,deptCode);
        }


        public bool SaveActualQty(string itemCode, string needQty, string stationeryDescription, string actualQty, string deptCode)
        {
            int aneedQty = Convert.ToInt32(needQty);
            int aactualQty = Convert.ToInt32(actualQty);
            try
            {
                List<WCFDisbursement> list = new List<WCFDisbursement>();
                if (HttpContext.Current.Application["tempDisbursement"] != null)
                {

                    list = (List<WCFDisbursement>)HttpContext.Current.Application["tempDisbursement"];

                }
                WCFDisbursement d = new WCFDisbursement();
                d.ItemCode = itemCode;
                d.NeedQty = aneedQty;
                d.StationeryDescription = stationeryDescription;
                d.DeptCode = deptCode;
                d.ActualQty = aactualQty;
                list.Add(d);
                HttpContext.Current.Application["tempDisbursement"] = list;
                return true;
            }
            catch
            {
                return false;
            }
            
        }

    //retrieve list must already be generated
        public List<WCFRequisitionRecord> GetAllRequestRecordForItemAllocation(string itemCode)
        {

                List<WCFRequisitionDetail> allRDForAllocation = GetAllRequisitionDetailsforAllocation().Where(x => x.ItemCode == itemCode).ToList();

                //if returns null "There is either no itemCode by that name or the retrieval list has not been generated, hence allocation cannot proceed"
            
            List<WCFRequisitionRecord> allRRForItem = new List<WCFRequisitionRecord>();

            //all record numbers for that item
            foreach (WCFRequisitionDetail WCFRd in allRDForAllocation)
            {

                allRRForItem.Add(WCFModelConvertUtility.ConvertToWCFRequisitionRecord(requisitionRecordService.GetRequisitionByID(WCFRd.RequisitionNo)));
                allRRForItem.Distinct().ToList();

            }

            ////all allocation records
            //List<Requisition_Record> records = (List<Requisition_Record>)HttpContext.Current.Application["requisitionRecordList_allocation"];
            //List<WCFRequisitionRecord> allocationRecords = WCFModelConvertUtility.ConvertToWCFRequisitionRecord(records).Where(x => x)

            

            return allRRForItem;
            
   
            
        }

        public List<WCFDisbursement> GetTMP()
        {
            return (List<WCFDisbursement>)HttpContext.Current.Application["tempDisbursement"];
        }

        public void UpdateDisbursement(string itemCode, string needQty, string actualQty, string DepartmentCode, string count, string staffID)
        {
            requisitionRecordService.UpdateDisbursement(itemCode, Convert.ToInt32(actualQty), DepartmentCode, Convert.ToInt32(needQty), Convert.ToInt32(count), staffID);
            HttpContext.Current.Application["tempDisbursement"] = null;
        }

        public void UpdateRequisitionDetail(WCFRequisitionDetail reqDetail)
        {

            requisitionRecordService.UpdateDetails(reqDetail.ItemCode, reqDetail.RequisitionNo, reqDetail.AllocateQty);

        }

        //public List<WCFDisbursement> GetCodeFromName(string name)
        //{
        //    stationeryService.GetAllStationery();

        //}


        //public List<Disbursement> getDisbursementList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<RequisitionRecord> getRequisitionListByUserID(string UserID)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<RequisitionDetails> getrequisitionDetailsByNO(int requisitionNo)
        //{
        //    throw new NotImplementedException();
        //}

    }
}

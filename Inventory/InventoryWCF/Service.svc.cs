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




        public WCFUser ValidateUser(WCFUser User)
        {
            //return BusinessLogic.validateUser(userid, password);

            try
            {
                User user = userService.FindByUserID(User.UserID);
                if (user != null)
                {
                    if (User.PassWord == Encrypt.DecryptMethod(user.password))
                    {
                        return WCFModelConvertUtility.ConvertToWCFUser(user);
                    } else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                // non existing userID
                return null;
            }           
        }

        public WCFUser GetUser(string userID, string password)
        {

            try
            {
                User user = userService.FindByUserID(userID);
                if (Encrypt.DecryptMethod(user.password) == password)
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
            
            try
            {
                if(ValidateUser(u) != null)
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


        public List<WCFRequisitionDetail> GetRequisitionDetailsByItemCode(string itemCode)
        {
            using(StationeryModel entity = new StationeryModel())
            {
                List<Requisition_Detail> temp = entity.Requisition_Detail.Where(x => x.itemCode == itemCode).ToList();
                return WCFModelConvertUtility.ConvertToWCFRequestionDetails(temp);
            }
      
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

        public List<WCFRequisitionRecord> GetRequisitionRecordByRequesterID(string requesterID)
        {
            List<Requisition_Record> reqByReqID = requisitionRecordService.GetRequestByReqID(requesterID);
            return WCFModelConvertUtility.ConvertToWCFRequisitionRecord(reqByReqID);
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
            catch (EmailException e)
            {
                // success submitssion 
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
        public Boolean updateRequisitionDetails(string requisitionNo, string ItemCode, string allocateQty)
        {
            int no = Int32.Parse(requisitionNo);
            int qty = Int32.Parse(allocateQty);
            return BusinessLogic.updateRequisitionDetails(no, ItemCode, qty);
        }

        public void UpdateRequisition (string requisitionNo, string status, string approveStaffID)
        {

            BusinessLogic.updateRequisition(Convert.ToInt32(requisitionNo) , status, approveStaffID);
        }

        public void UpdateCollectionPoint(string deptCode, string newcp)
        {
            try
            {
                BusinessLogic.updateCollectionPoint(deptCode, Convert.ToInt32(newcp));
                EmailNotification.EmailNotificationForCollectionPointChange(deptCode);
            }
            catch (EmailException e)
            {
                return;
            }
            catch (Exception e)
            {
                return;
            }
        }


        public List<WCFRetrievalForm> getRetrievalList()
        {
            StationeryModel entity = new StationeryModel();
            List<RetrieveForm> list = new List<RetrieveForm>();
             DateTime date = DateTime.Now;
            List<RetrieveForm> model = requisitionRecordService.GetRetrieveFormByDateTime(date);
            if (HttpContext.Current.Application["retrieveForm"] == null)
            {
                HttpContext.Current.Application["retrieveForm"] = model;
            }
            else if (HttpContext.Current.Application["retrieveForm"] != null)
            {
                List<RetrieveForm> list2 = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
                
                foreach (var item in model)
                {
                    foreach (var i in list2)
                    {
                        if (i.ItemCode == item.ItemCode)
                        {
                            item.retrieveQty = i.retrieveQty;
                        }
                    }
                }
                HttpContext.Current.Application["retrieveForm"] = model;

            }

            //generate list of requisition records for allocation at the same time
            List<Requisition_Record> rr = entity.Requisition_Records.Where(x => x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED).ToList();
            HttpContext.Current.Application["requisitionRecordList_allocation"] = rr;

            return WCFModelConvertUtility.ConvertToWCFRetrievalList(model);
            
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

        public string UpdateRetrieval(WCFRetrievalForm wcfr)
        {
            try
            {
                List<RetrieveForm> list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveForm"];
                RetrieveForm rf = list.Where(x => x.ItemCode == wcfr.ItemCode).First();
              
                Stationery item = stationeryService.FindStationeryByItemCode(wcfr.ItemCode);
                if(wcfr.QtyRetrieved.Value > item.stockQty)
                {
                    return "Value of Retrieved Qty cannot exceed Stock Qty.";
                }

                if(wcfr.QtyRetrieved > rf.Qty)
                {
                    return "Value of Retrieved Qty cannot exceed requested qty.";
                }

                var index = list.FindIndex(x => x.description == wcfr.Description);
                list[index].retrieveQty = wcfr.QtyRetrieved;
              /*  list[index].Qty -= wcfr.QtyRetrieved;*/ //reduce amount that is still pending retrieval

                HttpContext.Current.Application["retrieveForm"] = list;
                return "true";
            }

            catch(Exception e)
            {
                String error = e.Message;

                return error;
            }


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

        public List<WCFRequisitionDetail> GetConsolidatedRequisitionDetailsforAllocation()
        {

            List<string> itemCodes = stationeryService.GetListOfItemCodes();
            List<WCFRequisitionDetail> model = new List<WCFRequisitionDetail>();
            
            foreach (String ic in itemCodes) {
                
                int fulfilled = 0;
                int allocated = 0;
                int requested = 0;
          
                WCFRequisitionDetail rd = new WCFRequisitionDetail();
                List<WCFRequisitionDetail> list = GetRequisitionDetailsByItemCode(ic).Where(x => x.Status == "Approved and Processing" || x.Status == "Partially fulfilled").ToList();
       
                foreach(WCFRequisitionDetail itemRD in list)
                {
                    fulfilled += itemRD.FulfilledQty;
                    allocated += itemRD.AllocateQty;
                    requested += itemRD.Qty;
                    rd.StationeryDescription = itemRD.StationeryDescription;


                }
                rd.ItemCode = ic;
                rd.Qty = requested;
                rd.AllocateQty = allocated;
                rd.FulfilledQty = fulfilled;
                if (rd.Qty > rd.FulfilledQty)
                {
                    model.Add(rd);
                }


            }

           

           

            return model;
        }


        public void UpdateReqDetail(string No, string itemCode, string quantity)
        {
            int requisitionNo = Convert.ToInt32(No);
            int qty = Convert.ToInt32(quantity);
            using (StationeryModel entity = new StationeryModel())
            {
                entity.Requisition_Detail.Where(x => x.itemCode == itemCode && x.requisitionNo == requisitionNo).First().qty = (int)qty;
                entity.SaveChanges();
            }
        }

       

        public void RemovePendingRequisition(string NO)
        {
            int requisitionNo = Convert.ToInt32(NO);
            using (StationeryModel entity = new StationeryModel())
            {
                Requisition_Record record = (from r in entity.Requisition_Records
                                             where r.requisitionNo == requisitionNo
                                             select r).FirstOrDefault();

                entity.Requisition_Detail.RemoveRange(record.Requisition_Detail);
                entity.Requisition_Records.Remove(record);
                entity.SaveChanges();
            }
        }

        public List<WCFDisbursement> GetPendingItemsToBeProcessedByDepartmentByItems(string deptCode)
        {

            List<Disbursement> pendingItemsByItem = requisitionRecordService.GetPendingDisbursementByDept(deptCode);
            return WCFModelConvertUtility.ConvertToWCFDisbursement(pendingItemsByItem,deptCode);
        }


        public bool SaveActualQty(string itemCode, string needQty, string actualQty, string deptCode)
        {
            int aneedQty = Convert.ToInt32(needQty);
            int aactualQty = Convert.ToInt32(actualQty);
            Boolean status = true;
            try
            {
                List<WCFDisbursement> list = new List<WCFDisbursement>();
                if (HttpContext.Current.Application["tempDisbursement"] != null)
                {

                    list = (List<WCFDisbursement>)HttpContext.Current.Application["tempDisbursement"];
                    for (int i = 0;i<list.Count;i++)
                    {
                        if (list[i].DeptCode == deptCode && list[i].ItemCode == itemCode)
                        {
                            list[i].ActualQty = aactualQty;
                            status = false;
                            break;
                        }
                    }
                    if (status)
                    {
                        WCFDisbursement d = new WCFDisbursement();
                        d.ItemCode = itemCode;
                        d.NeedQty = aneedQty;
                        d.DeptCode = deptCode;
                        d.ActualQty = aactualQty;
                        list.Add(d);
                    }
                }else
                {
                    WCFDisbursement d = new WCFDisbursement();
                    d.ItemCode = itemCode;
                    d.NeedQty = aneedQty;
                    d.DeptCode = deptCode;
                    d.ActualQty = aactualQty;
                    list.Add(d);
                }
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

        public void UpdateRequisitionDetailAndroid(WCFRequisitionDetail reqDetail)
        {
            
            requisitionRecordService.UpdateDetailsAndroid(reqDetail.ItemCode, reqDetail.RequisitionNo, reqDetail.AllocateQty);

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

        public WCFDepartment GetDepartment(string departmentCode)
        {
            string deptCode = departmentCode.ToUpper().Trim();
            try
            {

                Department d = departmentService.GetDepartmentByCode(deptCode);
                if (d.departmentCode == deptCode)
                {
                    WCFDepartment WCFD = WCFModelConvertUtility.convertToWCFDepartment(d);
                    return WCFD;
                }
                else
                {
                    WCFDepartment invalid = new WCFDepartment();
                    return invalid;
                }
            }
            catch(Exception e)
            {
                WCFDepartment invalid = new WCFDepartment();
                return invalid;
            }

        }

    }
}

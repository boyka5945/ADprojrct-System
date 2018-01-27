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




        public Boolean ValidateUser(string userid, string password)
        {
            //return BusinessLogic.validateUser(userid, password);

            try
            {
                User user = userService.FindByUserID(userid);
                return (user.password == password); // wrong password
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
            // TODO : IMPLEMENT METHOD
            //return BusinessLogic.changePassWord(userid, currentpassword, newpassword);
            try
            {
                if(ValidateUser(userid, currentpassword))
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
             //need the list for the next delivery, a.k.a for next monday
             //must have been approved before this week's wednesday?

             //next delivery date supposedly is:
             DateTime date = DateTime.Now;
            //while(date.DayOfWeek != DayOfWeek.Monday)
            //{
            //    date.AddDays(1);
            //}
            if (HttpContext.Current.Application["retrieveList"] != null)
            {
                list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveList"];
            }
            else
            {
                list = requisitionRecordService.GetRetrieveFormByDateTime(date); //newly generated list
                HttpContext.Current.Application["retrieveList"] = list;
            }

            //generate list of requisition records for allocation at the same time
            List<Requisition_Record> rr = entity.Requisition_Records.Where(x => x.approveDate < date && (x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED)).ToList();
            HttpContext.Current.Application["requisitionRecordList_allocation"] = rr;

            return WCFModelConvertUtility.ConvertToWCFRetrievalList(list);
            
        }

        public bool UpdateRetrieval(WCFRetrievalForm wcfr)
        {
            List<RetrieveForm> list = (List<RetrieveForm>)HttpContext.Current.Application["retrieveList"];
            //RetrieveForm rf = list.Where(x => x.description == description).First();
            //rf.retrieveQty = Int32.Parse(qty);
            var index = list.FindIndex(x => x.description == wcfr.Description);
            list[index].retrieveQty = wcfr.QtyRetrieved;

            HttpContext.Current.Application["retrieveList"] = list;
            return true;


        }


        public List<WCFDepartment> GetAllDepartments()
        {
            List<Department> departmentList = departmentService.GetAllDepartment();
            return WCFModelConvertUtility.ConvertToWCFDepartments(departmentList);


        }

        public List<WCFDisbursement> GetDisbursementByDept(string deptCode)
        {
            List<Disbursement> disbursement = requisitionRecordService.GetRequisitionByDept(deptCode);
            return WCFModelConvertUtility.ConvertToWCFDisbursement(disbursement);
        }

        //allocation list should match the retrieval list
        public List<WCFRequisitionDetail> GetAllRequisitionDetailsforAllocation()
        {
            List<WCFRequisitionDetail> allocationList = new List<WCFRequisitionDetail>();
            if(HttpContext.Current.Application["requisitionRecordList_allocation"] == null)
            {
                return allocationList = null; //empty
            }

            List<Requisition_Record> records = (List<Requisition_Record>)HttpContext.Current.Application["requisitionRecordList_allocation"];

            foreach(Requisition_Record rr in records)
            {
                List<Requisition_Detail> temp = (List<Requisition_Detail>) rr.Requisition_Detail;
                List<WCFRequisitionDetail> wcftemp = WCFModelConvertUtility.ConvertToWCFRequestionDetails(temp);
                allocationList.AddRange(wcftemp);
            }

            return allocationList;




        }


        public bool SaveActualQty(WCFDisbursement wcfd)
        {
            try
            {
                List<WCFDisbursement> list = new List<WCFDisbursement>();
                if (HttpContext.Current.Application["tempDisbursement"] != null)
                {

                    list = (List<WCFDisbursement>)HttpContext.Current.Application["tempDisbursement"];

                }
                WCFDisbursement d = new WCFDisbursement();
                d.ItemCode = wcfd.ItemCode;
                d.NeedQty = wcfd.NeedQty;
                d.StationeryDescription = wcfd.StationeryDescription;
                d.ActualQty = wcfd.ActualQty;
                list.Add(d);
                HttpContext.Current.Application["tempDisbursement"] = list;
                return true;
            }
            catch
            {
                return false;
            }
            
        }

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

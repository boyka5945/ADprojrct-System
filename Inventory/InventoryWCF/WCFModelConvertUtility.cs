using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Service;

namespace InventoryWCF
{
    public static class WCFModelConvertUtility
    {
        private static IStationeryService stationeryService = new StationeryService();


        public static List<WCFDisbursement> ConvertToWCFDisbursement(List<Disbursement> disbursement, string deptCode)
        {
            List<WCFDisbursement> dl = new List<WCFDisbursement>();
            foreach (var list in disbursement)
            {
                WCFDisbursement d = new WCFDisbursement();
                d.ItemCode = list.itemCode;
                d.StationeryDescription = list.itemDescription;
                d.NeedQty = list.quantity;
                if (HttpContext.Current.Application["tempDisbursement"] != null)
                {
                    List<WCFDisbursement> wcfl = (List<WCFDisbursement>)HttpContext.Current.Application["tempDisbursement"];
                    if (wcfl.Where(x => x.ItemCode == list.itemCode && x.DeptCode == deptCode).Count() > 0)
                        d.ActualQty = wcfl.Where(x => x.ItemCode == list.itemCode && x.DeptCode == deptCode).First().ActualQty;
                    else
                        d.ActualQty = 0;
                }
                else
                {
                    d.ActualQty = 0;
                }
                d.DeptCode = deptCode;
                dl.Add(d);
            }
            return dl;
        }


        public static WCFRequisitionDetail ConvertToWCFRequisitionDetail(Requisition_Detail detail)
        {
            Stationery s = stationeryService.FindStationeryByItemCode(detail.itemCode);
            WCFRequisitionDetail wcf_detail = new WCFRequisitionDetail();
            wcf_detail.RequisitionNo = detail.requisitionNo;
            wcf_detail.ItemCode = detail.itemCode;
            wcf_detail.StationeryDescription = s.description;
            wcf_detail.UOM = s.unitOfMeasure;
            wcf_detail.Remarks = detail.remarks;
            wcf_detail.Qty = (detail.qty == null) ? 0 : (int) detail.qty;
            wcf_detail.FulfilledQty = (detail.fulfilledQty == null) ? 0 : (int)detail.fulfilledQty;
            wcf_detail.ClerkID = detail.clerkID;
            wcf_detail.RetrievedDate = detail.retrievedDate;
            wcf_detail.AllocateQty = (detail.allocatedQty == null) ? 0 : (int)detail.allocatedQty;
            wcf_detail.NextCollectionDate = detail.nextCollectionDate;
            wcf_detail.Status = detail.Requisition_Record.status;

            return wcf_detail;
        }

        public static List<WCFRequisitionDetail> ConvertToWCFRequisitionDetail(List<Requisition_Detail> requisitionDetail)
        {
            List<WCFRequisitionDetail> wcf_requisitionDetail = new List<WCFRequisitionDetail>();
            foreach (var rd in requisitionDetail)
            {
                wcf_requisitionDetail.Add(ConvertToWCFRequisitionDetail(rd));
            }

            return wcf_requisitionDetail;
        }

        public static WCFRequisitionRecord ConvertToWCFRequisitionRecord(Requisition_Record requisitionRecord)
        {
            WCFRequisitionRecord wcf_requisitionRecord = new WCFRequisitionRecord();
            wcf_requisitionRecord.RequisitionNo = requisitionRecord.requisitionNo;
            wcf_requisitionRecord.DeptCode = requisitionRecord.deptCode;
            wcf_requisitionRecord.DeptName = requisitionRecord.Department.departmentName;
            wcf_requisitionRecord.RequesterID = requisitionRecord.requesterID;
            wcf_requisitionRecord.RequesterName = requisitionRecord.User.name;
            wcf_requisitionRecord.ApprovingStaffID = requisitionRecord.approvingStaffID;
            wcf_requisitionRecord.ApprovingStaffName = requisitionRecord.User.name;
            wcf_requisitionRecord.ApproveDate = requisitionRecord.approveDate;
            wcf_requisitionRecord.Status = requisitionRecord.status;
            wcf_requisitionRecord.RequestDate = requisitionRecord.requestDate;
            
            return wcf_requisitionRecord;
        }

        public static Requisition_Record ConvertFromRequisitionRecord(WCFRequisitionRecord wcfRecRecord)
        {
            Requisition_Record recRecord = new Requisition_Record();
            recRecord.requisitionNo = wcfRecRecord.RequisitionNo;
            recRecord.deptCode = wcfRecRecord.DeptCode;
            recRecord.Department.departmentName = wcfRecRecord.DeptName;
            recRecord.requesterID = wcfRecRecord.RequesterID;
            recRecord.User.name = wcfRecRecord.RequesterName;
            recRecord.approvingStaffID = wcfRecRecord.ApprovingStaffID;
            recRecord.User.name = wcfRecRecord.ApprovingStaffName;
            recRecord.approveDate = wcfRecRecord.ApproveDate;
            recRecord.status = wcfRecRecord.Status;
            recRecord.requestDate = wcfRecRecord.RequestDate;

            return recRecord;
        }

        public static List<WCFRequisitionRecord> ConvertToWCFRequisitionRecord(List<Requisition_Record> requisitionRecords)
        {
            List<WCFRequisitionRecord> wcf_requisitionRecords = new List<WCFRequisitionRecord>();
            foreach (var rr in requisitionRecords)
            {
                wcf_requisitionRecords.Add(ConvertToWCFRequisitionRecord(rr));
            }

            return wcf_requisitionRecords;
        }
        public static WCFRetrievalForm ConvertToWCFRetrievalForm()
        {
            // TODO : IMPLEMENT METHOD
            throw new NotImplementedException();
        }

        public static WCFStationery ConvertToWCFStationery(Stationery stationery)
        {
            WCFStationery wcf_stationery = new WCFStationery();
            wcf_stationery.CategoryName = stationery.Category.categoryName;
            wcf_stationery.Description = stationery.description;
            wcf_stationery.ItemCode = stationery.itemCode;
            wcf_stationery.Location = stationery.location;
            wcf_stationery.UOM = stationery.unitOfMeasure;

            return wcf_stationery;
        }

        public static List<WCFStationery> ConvertToWCFStationery(List<Stationery> stationeries)
        {
            List<WCFStationery> wcf_stationeries = new List<WCFStationery>();
            foreach(var s in stationeries)
            {
                wcf_stationeries.Add(ConvertToWCFStationery(s));
            }

            return wcf_stationeries;
        }


        public static WCFUser ConvertToWCFUser(User user)
        {
            WCFUser wUser = new WCFUser();
            wUser.UserID = user.userID;
            wUser.DepartmentCode = user.departmentCode;
            wUser.Role = user.role;
            wUser.Name = user.name;

            return wUser;
 
        }

        public static WCFCategory ConvertToWCFCategory(Category category)
        {
            WCFCategory wcf_category = new WCFCategory();
            wcf_category.CategoryID = category.categoryID;
            wcf_category.CategoryName = category.categoryName;

            return wcf_category;
        }


        public static List<WCFCategory> ConvertToWCFCategories(List<Category> categories)
        {
            List<WCFCategory> wcf_categories = new List<WCFCategory>();
            foreach(var c in categories)
            {
                wcf_categories.Add(ConvertToWCFCategory(c));
            }

            return wcf_categories;
        }

        public static WCFDepartment convertToWCFDepartment(Department d)
        {
            WCFDepartment wcfd = new WCFDepartment();
            wcfd.DepartmentCode = d.departmentCode;
            wcfd.DepartmentName = d.departmentName;
            wcfd.ContactName = d.contactName;
            wcfd.CollectionPointID = d.collectionPointID;
            wcfd.CollectionPointName = d.Collection_Point.collectionPointName;

            return wcfd;
        }

        public static List<WCFDepartment> ConvertToWCFDepartments (List<Department> departments)
        {
            List<WCFDepartment> wcf_departments = new List<WCFDepartment>();
            foreach (Department d in departments)
            {
                wcf_departments.Add(convertToWCFDepartment(d));
            }
            return wcf_departments;
        }

        public static WCFCollectionPoint convertToWCFCollectionPoint(Collection_Point cp)
        {
            WCFCollectionPoint wcfcp = new WCFCollectionPoint();
            wcfcp.Collection_Point_Name = cp.collectionPointName;
            wcfcp.Collection_Point_ID = cp.collectionPointID;
           

            return wcfcp;
        }

        public static List<WCFCollectionPoint> convertToWCFCollectionPoints(List<Collection_Point> cps)
        {
            List<WCFCollectionPoint> wcf_cp = new List<WCFCollectionPoint>();
            foreach (Collection_Point cp in cps)
            {
                wcf_cp.Add(convertToWCFCollectionPoint(cp));
            }
            return wcf_cp;
        }

        public static WCFRetrievalForm ConvertToWCFRetrieval (RetrieveForm retrieval)
        {
            WCFRetrievalForm wcfR = new WCFRetrievalForm();
            
            wcfR.Description = retrieval.description;
            wcfR.Qty = retrieval.Qty;
            wcfR.QtyRetrieved = retrieval.retrieveQty;
            //wcfR.QtyAllocated = retrieval.allocatedQty;
            wcfR.ItemCode = retrieval.ItemCode;
            //add location to WCFRetrievalForm object
            StationeryModel entity = new StationeryModel();
            wcfR.Location = entity.Stationeries.Where(x => x.description == retrieval.description).First().location;
         

            return wcfR;
           
        }

        public static List<WCFRetrievalForm> ConvertToWCFRetrievalList (List<RetrieveForm> retrieveForm)
        {
            List<WCFRetrievalForm> wcf_retrievals = new List<WCFRetrievalForm>();
            foreach(RetrieveForm r in retrieveForm)
            {
                wcf_retrievals.Add(ConvertToWCFRetrieval(r));
            }
            return wcf_retrievals;
        }

        public static Requisition_Detail ConvertFromWCFRequisitionDetail(WCFRequisitionDetail wcf_detail)
        {
            Requisition_Detail detail = new Requisition_Detail();
            detail.allocatedQty = wcf_detail.AllocateQty;
            detail.clerkID = wcf_detail.ClerkID;
            detail.fulfilledQty = wcf_detail.FulfilledQty;
            detail.itemCode = wcf_detail.ItemCode;
            detail.nextCollectionDate = wcf_detail.NextCollectionDate;
            detail.qty = wcf_detail.Qty;
            detail.remarks = wcf_detail.Remarks;
            detail.requisitionNo = wcf_detail.RequisitionNo;
            detail.retrievedDate = wcf_detail.RetrievedDate;

            return detail;
        }
        

       
        public static List<WCFRequisitionDetail> ConvertToWCFRequestionDetails(List<Requisition_Detail> requestionDetail)
        {
            List<WCFRequisitionDetail> wcf_reqDetail = new List<WCFRequisitionDetail>();
            foreach(var rd in requestionDetail)
            {
                wcf_reqDetail.Add(ConvertToWCFRequisitionDetail(rd));
            }

            return wcf_reqDetail;
        }

        
    }
}
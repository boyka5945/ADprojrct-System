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


        public static WCFDisbursement ConvertToWCFDisbursement()
        {
            // TODO : IMPLEMENT METHOD
            throw new NotImplementedException();
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

            return wcf_detail;
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

        
    }
}
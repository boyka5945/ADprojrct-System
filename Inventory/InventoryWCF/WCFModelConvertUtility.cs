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

        public static WCFRequisitionRecord ConvertToWCFRequisitionRecord()
        {
            throw new NotImplementedException();
        }

        public static WCFRetrievalForm ConvertToWCFRetrievalForm()
        {
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


        public static WCFUser ConvertToWCFUser()
        {
            throw new NotImplementedException();
        }

    }
}
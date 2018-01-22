using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.ViewModel;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Service
{
    public class InventoryStatusRecordService : IInventoryStatusRecordService
    {
        IInventoryStatusRecordDAO inventoryDAO = new InventoryStatusRecordDAO();
        IStationeryService stationeryService = new StationeryService();
        IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();

        public List<InventoryCheckViewModel> GetInventoryChecklistBasedOnCategory(int[] categoryID)
        {
            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCategoryID(categoryID);

            List<InventoryCheckViewModel> vmList = new List<InventoryCheckViewModel>();
            foreach(var s in stationeries)
            {
                InventoryCheckViewModel vm = new InventoryCheckViewModel();
                vm.CategoryName = s.Category.categoryName;
                vm.CategoryID = s.categoryID;
                vm.StationeryDescription = s.description;
                vm.ItemCode = s.itemCode;
                vm.Location = s.location;
                vm.StockQuantity = s.stockQty;
                vm.UOM = s.unitOfMeasure;
                vm.ActualQuantity = s.stockQty;

                vmList.Add(vm);
            }

            return vmList;
        }

        public List<InventoryCheckViewModel> ConvertStockChecklistToDiscrepancyList(List<InventoryCheckViewModel> stockchecklist)
        {
            List<InventoryCheckViewModel> discrepancyList = new List<InventoryCheckViewModel>();

            foreach(var item in stockchecklist)
            {
                if(item.ActualQuantity != item.StockQuantity)
                {
                    item.Discrepancy = item.ActualQuantity - item.StockQuantity;
                    discrepancyList.Add(item);
                }
            }

            return discrepancyList;
        }

        public bool SaveInventoryCheckResult(List<InventoryCheckViewModel> stockchecklist)
        {
            List<Inventory_Status_Record> records = new List<Inventory_Status_Record>();
            foreach(var item in stockchecklist)
            {
                Inventory_Status_Record record = new Inventory_Status_Record();
                record.date = DateTime.Today;
                record.discrepancyQty = item.Discrepancy;
                record.itemCode = item.ItemCode;
                record.onHandQty = item.StockQuantity;
                record.remarks = item.Remarks;

                records.Add(record);
            }

            if(inventoryDAO.AddNewInventoryStatusRecords(records))
            {
                return true;
            }
            else
            {
                throw new Exception("Error when writing inventory status records");
            }

        }

        public bool SubmitAdjustmentVoucherForInventoryCheckDiscrepancy(List<InventoryCheckViewModel> stockchecklist, string requesterID)
        {
            List<InventoryCheckViewModel> discrepancyList = ConvertStockChecklistToDiscrepancyList(stockchecklist);

            List<AdjustmentVoucherViewModel> vmList = new List<AdjustmentVoucherViewModel>();
            foreach(var item in discrepancyList)
            {
                AdjustmentVoucherViewModel vm = new AdjustmentVoucherViewModel();
                vm.ItemCode = item.ItemCode;
                vm.Quantity = item.Discrepancy;
                vm.Reason = item.Remarks;                
            }

            if(adjustmentVoucherService.SubmitNewAdjustmentVoucher(vmList, AdjustmentVoucherRemarks.INV_CHECK, requesterID))
            {
                return true;
            }
            else
            {
                throw new Exception("Error when submitting adjustment voucher");
            }
        }

    }
}
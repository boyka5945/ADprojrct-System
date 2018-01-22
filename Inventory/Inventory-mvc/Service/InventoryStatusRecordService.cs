using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.ViewModel;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public class InventoryStatusRecordService : IInventoryStatusRecordService
    {
        IInventoryStatusRecordDAO inventoryDAO = new InventoryStatusRecordDAO();
        IStationeryService stationeryService = new StationeryService();

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
    }
}
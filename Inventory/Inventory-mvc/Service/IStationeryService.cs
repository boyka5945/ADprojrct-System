using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public interface IStationeryService
    {
        // FOR STATIONERY
        List<Stationery> GetAllStationery();

        bool AddNewStationery(StationeryViewModel stationeryVM);

        bool UpdateStationeryInfo(StationeryViewModel stationeryVM);

        bool DeleteStationery(string itemCode);

        Stationery FindStationeryByItemCode(string itemCode);

        bool UpdateStationeryQuantity(string itemCode, int adjustedQuantity);

        List<Stationery> GetStationeriesBasedOnCategoryID(int[] categoryID);

        // HELPER METHOD
        bool isExistingCode(string itemCode);

        bool isPositiveLevel(int reorderLevel);

        bool isPositiveQty(int reorderQty);

        bool isPositivePrice(decimal price);

        bool isExistingSupplierCode(string supplier1, string supplier2);

        List<string> GetAllUOMList();

        List<string> GetAllFirstSupplierList();

        List<string> GetAllSecondSupplierList();

        List<string> GetAllThirdSupplierList();

        List<int> GetAllCategoryIDList();

        List<Category> GetAllCategory();

        List<Stationery> GetStationeriesBasedOnCriteria(string searchString, string categoryID);

        List<Stationery> GetStationeriesBasedOnCriteria(string itemCodeOrDescription);

        // FOR VIEW MODEL
        List<StationeryViewModel> GetAllStationeryViewModel();

        StationeryViewModel FindStationeryViewModelByItemCode(string itemCode);

        List<StationeryViewModel> GetStationeriesVMBasedOnCriteria(string searchString, string categoryID);

        List<StationeryViewModel> GetAllItemCodes();

        // TODO - REMOVE THIS METHOD
        List<string> GetListOfItemCodes();
    }
}
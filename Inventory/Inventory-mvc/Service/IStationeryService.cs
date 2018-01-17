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


        // HELPER METHOD
        bool isExistingCode(string itemCode);

        bool isPositiveLevel(int reorderLevel);

        bool isPositiveQty(int reorderQty);

        bool isPositivePrice(decimal price);

        List<Category> GetAllCategory();


        // FOR VIEW MODEL
        List<StationeryViewModel> GetAllStationeryViewModel();

        StationeryViewModel FindStationeryViewModelByItemCode(string itemCode);

    }
}
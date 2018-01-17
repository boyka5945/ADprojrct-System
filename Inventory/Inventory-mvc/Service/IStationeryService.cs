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
        List<StationeryViewModel> GetAllStationeryViewModel();

        bool AddNewStationery(StationeryViewModel stationeryVM);

        bool DeleteStationery(string itemCode);

        StationeryViewModel FindByItemCode(string itemCode);

        bool UpdateStationeryInfo(StationeryViewModel stationeryVM);
        bool isExistingCode(string itemCode);

        List<Stationery> GetAllStationery();

        List<Category> GetAllCategory();
    }
}
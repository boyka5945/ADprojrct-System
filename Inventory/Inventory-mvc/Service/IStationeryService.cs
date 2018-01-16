using Inventory_mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Service
{
    public interface IStationeryService
    {
        List<StationeryViewModel> GetAllStationery();
        bool AddNewStationery(StationeryViewModel stationeryVM);

        bool DeleteStationery(string itemCode);

        StationeryViewModel FindByItemCode(string itemCode);

         bool UpdateStationeryInfo(StationeryViewModel stationeryVM);
         bool isExistingCode(string itemCode);
        
    
        
    }
}
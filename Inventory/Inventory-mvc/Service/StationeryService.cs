using Inventory_mvc.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Service
{
    public class StationeryService
    {
        private IStationeryDAO stationeryDAO = new StationeryDAO();

        bool IStationeryService.AddNewStationery(StationeryViewModel stationeryVM)
        {
            return stationeryDAO.AddNewStationery(ConvertFromViewModel(stationeryVM));
        }
    }
}
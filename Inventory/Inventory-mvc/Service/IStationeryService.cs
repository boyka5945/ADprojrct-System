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
    }
}
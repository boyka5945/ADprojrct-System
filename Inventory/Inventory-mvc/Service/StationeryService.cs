using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
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

        private StationeryDAO ConvertFromViewModel(StationeryViewModel stationeryVM)
        {
            Stationery stationery = new Stationery();

            stationery.itemCode = stationeryVM.ItemCode;
            stationery.categoryID = stationeryVM.GSTNo;
            supplier.supplierName = supplierVM.SupplierName;
            supplier.contactName = supplierVM.ContactName;
            supplier.phoneNo = (int)supplierVM.PhoneNo;
            supplier.faxNo = supplierVM.FaxNo;
            supplier.address = supplierVM.Address;

            return stationery;
        }
    }
}
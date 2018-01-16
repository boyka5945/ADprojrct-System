using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Service
{
    public class StationeryService : IStationeryService
    {
        //private IStationeryDAO stationeryDAO = new StationeryDAO();


        List<StationeryViewModel> IStationeryService.GetAllSuppliers()
        {
            List<Stationery> stationeryList = stationeryDAO.GetAllSupplier();

            List<StationeryViewModel> viewModelList = new List<StationeryViewModel>();
            foreach (Stationery s in stationeryList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        private StationeryViewModel ConvertToViewModel(Stationery s)
        {
            throw new NotImplementedException();
        }
        private IStationeryDAO stationeryDAO = new StationeryDAO();

        

        bool IStationeryService.AddNewStationery(StationeryViewModel stationeryVM)
        {
            return stationeryDAO.AddNewStationery(ConvertFromViewModel(stationeryVM));
        }

        private StationeryDAO ConvertFromViewModel(StationeryViewModel stationeryVM)
        {
            throw new NotImplementedException();

        }

        bool IStationeryService.UpdateStationeryInfo(StationeryViewModel stationeryVM)
        {
            throw new NotImplementedException();

        }

        StationeryViewModel IStationeryService.FindByItemCode(string itemCode)
        {
            throw new NotImplementedException();
        }

        bool IStationeryService.isExistingCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();

            return stationeryDAO.GetAllStationeryCode().Contains(code);
        }

            Stationery stationery = new Stationery();

            stationery.itemCode = stationeryVM.ItemCode;
            stationery.categoryID = (int)stationeryVM.CategoryID;
            stationery.reorderLevel = (int)stationeryVM.ReorderLevel;
            stationery.reorderQty = (int)stationeryVM.ReorderQty;
            stationery.categoryID = stationeryVM.CategoryID;
            stationery.description = stationeryVM.Description;
            stationery.reorderLevel = stationeryVM.ReorderLevel;
            stationery.reorderQty = stationeryVM.ReorderQty;
            stationery.unitOfMeasure = stationeryVM.UnitOfMeasure;
            stationery.stockQty = (int)stationeryVM.StockQty;
            stationery.stockQty = stationeryVM.StockQty;
            stationery.location = stationeryVM.Location;
            stationery.firstSupplierCode = stationeryVM.FirstSupplierCode;
            stationery.price = (decimal)stationeryVM.Price;
            stationery.secondSupplierCode = stationeryVM.SecondSupplierCode;
            stationery.thirdSupplierCode = stationeryVM.ThirdSupplierCode;
            stationery.price = stationeryVM.Price;
            return stationery;
        }
    }
}
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
        private IStationeryDAO stationeryDAO = new StationeryDAO();


        List<StationeryViewModel> IStationeryService.GetAllStationery()
        {
            List<Stationery> stationeryList = stationeryDAO.GetAllStationery();

            List<StationeryViewModel> viewModelList = new List<StationeryViewModel>();
            foreach (Stationery s in stationeryList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        private StationeryViewModel ConvertToViewModel(Stationery s)
        {
            StationeryViewModel stationeryVM = new StationeryViewModel();

            stationeryVM.ItemCode = s.itemCode;
            stationeryVM.CategoryID = s.categoryID;
            stationeryVM.ReorderLevel = s.reorderLevel;
            stationeryVM.ReorderQty = s.reorderQty;
            stationeryVM.UnitOfMeasure = s.unitOfMeasure;
            stationeryVM.StockQty = s.stockQty;
            stationeryVM.FirstSupplierCode = s.firstSupplierCode;
            stationeryVM.Price =  s.price;
            return stationeryVM;
        }


        bool IStationeryService.AddNewStationery(StationeryViewModel stationeryVM)
        {
            return stationeryDAO.AddNewStationery(ConvertFromViewModel(stationeryVM));
        }

        bool IStationeryService.DeleteStationery(string itemCode)
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


        private StationeryDAO ConvertFromViewModel(StationeryViewModel stationeryVM)
        {
            Stationery stationery = new Stationery();
            stationery.itemCode = stationeryVM.ItemCode;
            stationery.categoryID = (int)stationeryVM.CategoryID;
            stationery.description = stationeryVM.Description;
            stationery.reorderLevel = (int)stationeryVM.ReorderLevel;
            stationery.reorderQty = (int)stationeryVM.ReorderQty;
            stationery.unitOfMeasure = stationeryVM.UnitOfMeasure;
            stationery.stockQty = (int)stationeryVM.StockQty;
            stationery.location = stationeryVM.Location;
            stationery.firstSupplierCode = stationeryVM.FirstSupplierCode;
            stationery.secondSupplierCode = stationeryVM.SecondSupplierCode;
            stationery.thirdSupplierCode = stationeryVM.ThirdSupplierCode;
            stationery.price = (decimal)stationeryVM.Price;
            return stationery;
        }


    }
}
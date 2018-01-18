using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class StationeryService : IStationeryService
    {
        private IStationeryDAO stationeryDAO = new StationeryDAO();


        List<StationeryViewModel> IStationeryService.GetAllStationeryViewModel()
        {
            List<Stationery> stationeryList = stationeryDAO.GetAllStationery();

            List<StationeryViewModel> viewModelList = new List<StationeryViewModel>();
            foreach (Stationery s in stationeryList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        bool IStationeryService.isExistingCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();

            return stationeryDAO.GetAllItemCode().Contains(code);
        }

        bool IStationeryService.isPositiveLevel(int reorderLevel)
        {
            int level = reorderLevel;
            if (level < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IStationeryService.isPositivePrice(decimal price)
        {
            decimal p = price;
            if (p < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IStationeryService.isPositiveQty(int reorderQty)
        {
            int qty = reorderQty;
            if (qty < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IStationeryService.AddNewStationery(StationeryViewModel stationeryVM)
        {
            return stationeryDAO.AddNewStationery(ConvertFromViewModel(stationeryVM));
        }

        private StationeryViewModel ConvertToViewModel(Stationery s)
        {
            StationeryViewModel stationeryVM = new StationeryViewModel();

            stationeryVM.ItemCode = s.itemCode;
            stationeryVM.CategoryID = s.categoryID;
            stationeryVM.Description = s.description;
            stationeryVM.ReorderLevel = s.reorderLevel;
            stationeryVM.ReorderQty = s.reorderQty;
            stationeryVM.UnitOfMeasure = s.unitOfMeasure;
            stationeryVM.StockQty = s.stockQty;
            stationeryVM.Location = s.location;
            stationeryVM.FirstSupplierCode = s.firstSupplierCode;
            stationeryVM.SecondSupplierCode = s.secondSupplierCode;
            stationeryVM.ThirdSupplierCode = s.thirdSupplierCode;
            stationeryVM.Price = s.price;
            stationeryVM.Category = s.Category;
            return stationeryVM;
        }    

        

        private Stationery ConvertFromViewModel(StationeryViewModel stationeryVM)
        {
            Stationery stationery = new Stationery();

            stationery.itemCode = stationeryVM.ItemCode;
            stationery.categoryID = stationeryVM.CategoryID;
            stationery.description = stationeryVM.Description;
            stationery.reorderLevel = stationeryVM.ReorderLevel;
            stationery.reorderQty = stationeryVM.ReorderQty;
            stationery.unitOfMeasure = stationeryVM.UnitOfMeasure;
            stationery.stockQty = stationeryVM.StockQty;
            stationery.location = stationeryVM.Location;
            stationery.firstSupplierCode = stationeryVM.FirstSupplierCode;
            stationery.secondSupplierCode = stationeryVM.SecondSupplierCode;
            stationery.thirdSupplierCode = stationeryVM.ThirdSupplierCode;
            stationery.price = stationeryVM.Price;
            return stationery;

        }

        bool IStationeryService.UpdateStationeryInfo(StationeryViewModel stationeryVM)
        {
            Stationery stationery = ConvertFromViewModel(stationeryVM);

            IStationeryDAO stationeryDAO = new StationeryDAO();

            if (stationeryDAO.UpdateStationeryInfo(stationery) == 1)
            {
                return true;
            } else
            {
                return false;
            }

        }

        StationeryViewModel IStationeryService.FindStationeryViewModelByItemCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            return ConvertToViewModel(stationeryDAO.FindByItemCode(code));
        }

        Stationery IStationeryService.FindStationeryByItemCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            return stationeryDAO.FindByItemCode(code);
        }



        bool IStationeryService.DeleteStationery(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            Stationery stationery = stationeryDAO.FindByItemCode(code);

            if (stationery.Requisition_Detail.Count != 0 || stationery.Purchase_Detail.Count != 0 || stationery.Voucher_Details.Count != 0)
            {
                return false;
            }

            return stationeryDAO.DeleteStationery(code);
        }

        
        List<Stationery> IStationeryService.GetAllStationery()
        {
            return stationeryDAO.GetAllStationery();
        }

        List<Category> IStationeryService.GetAllCategory()
        {
            return stationeryDAO.GetAllCategory();
        }


        List<string> IStationeryService.GetAllUOMList()
        {
            return stationeryDAO.GetAllUOMList();
        }

        List<string> IStationeryService.GetAllFirstSupplierList()
        {
            return stationeryDAO.GetAllFirstSupplierList();
        }

        List<string> IStationeryService.GetAllSecondSupplierList()
        {
            return stationeryDAO.GetAllSecondSupplierList();
        }

        List<string> IStationeryService.GetAllThirdSupplierList()
        {
            return stationeryDAO.GetAllThirdSupplierList();
        }

        List<int> IStationeryService.GetAllCategoryIDList()
        {
            return stationeryDAO.GetAllCategoryIDList();
        }

        bool IStationeryService.isExistingSupplierCode(string supplier1, string supplier2)
        {
            string code1 = supplier1.ToUpper().Trim();
            string code2 = supplier2.ToUpper().Trim();
           // string code3 = supplier3.ToUpper().Trim();
            if (code1 == code2 )
            { return true; }
            else { return false; }
        }

    }
}
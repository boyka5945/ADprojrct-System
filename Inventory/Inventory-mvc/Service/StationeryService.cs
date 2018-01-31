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


        public List<StationeryViewModel> GetAllStationeryViewModel()
        {
            List<Stationery> stationeryList = stationeryDAO.GetAllStationery();

            List<StationeryViewModel> viewModelList = new List<StationeryViewModel>();
            foreach (Stationery s in stationeryList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        public bool isExistingCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();

            return stationeryDAO.GetAllItemCode().Contains(code);
        }

        public bool isPositiveLevel(int reorderLevel)
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

        public bool isPositivePrice(decimal price)
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

        public bool isPositiveQty(int reorderQty)
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

        public bool AddNewStationery(StationeryViewModel stationeryVM)
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

        public bool UpdateStationeryInfo(StationeryViewModel stationeryVM)
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

        public StationeryViewModel FindStationeryViewModelByItemCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            return ConvertToViewModel(stationeryDAO.FindByItemCode(code));
        }

        public Stationery FindStationeryByItemCode(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            return stationeryDAO.FindByItemCode(code);
        }



        public bool DeleteStationery(string itemCode)
        {
            string code = itemCode.ToUpper().Trim();
            Stationery stationery = stationeryDAO.FindByItemCode(code);

            if (stationery.Requisition_Detail.Count != 0 || stationery.Purchase_Detail.Count != 0 || stationery.Voucher_Details.Count != 0)
            {
                return false;
            }

            return stationeryDAO.DeleteStationery(code);
        }


        public List<Stationery> GetAllStationery()
        {
            return stationeryDAO.GetAllStationery();
        }

        public List<Category> GetAllCategory()
        {
            return stationeryDAO.GetAllCategory();
        }


        public List<string> GetAllUOMList()
        {
            return stationeryDAO.GetAllUOMList();
        }

        public List<string> GetAllFirstSupplierList()
        {
            return stationeryDAO.GetAllFirstSupplierList();
        }

        public List<string> GetAllSecondSupplierList()
        {
            return stationeryDAO.GetAllSecondSupplierList();
        }

        public List<string> GetAllThirdSupplierList()
        {
            return stationeryDAO.GetAllThirdSupplierList();
        }

        public List<int> GetAllCategoryIDList()
        {
            return stationeryDAO.GetAllCategoryIDList();
        }

        public bool isExistingSupplierCode(string supplier1, string supplier2)
        {
            string code1 = supplier1.ToUpper().Trim();
            string code2 = supplier2.ToUpper().Trim();
           // string code3 = supplier3.ToUpper().Trim();
            if (code1 == code2 )
            { return true; }
            else { return false; }
        }

        public List<Stationery> GetStationeriesBasedOnCriteria(string searchString, string categoryID)
        {
            return stationeryDAO.GetStationeriesBasedOnCriteria(searchString, categoryID);
        }

        public List<StationeryViewModel> GetStationeriesVMBasedOnCriteria(string searchString, string categoryID)
        {
            List<Stationery> stationeries = GetStationeriesBasedOnCriteria(searchString, categoryID);

            List<StationeryViewModel> vmList = new List<StationeryViewModel>();

            foreach(var s in stationeries)
            {
                vmList.Add(ConvertToViewModel(s));
            }

            return vmList;
        }

        public List<Stationery> GetStationeriesBasedOnCriteria(string itemCodeOrDescription)
        {
            return stationeryDAO.GetStationeriesBasedOnCriteria(itemCodeOrDescription);
        }

        public bool UpdateStationeryQuantity(string itemCode, int adjustedQuantity)
        {
            Stationery s = FindStationeryByItemCode(itemCode);
            s.stockQty += adjustedQuantity;

            if(s.stockQty < 0)
            {
                string errorMessage = String.Format("Stock quantity of {0} is less than 0", itemCode);
                throw new Exception(errorMessage);
            }

            return (stationeryDAO.UpdateStationeryInfo(s) == 1) ? true : false;
        }

        List<StationeryViewModel> IStationeryService.GetAllItemCodes()
        {
            List<Stationery> itemCode = stationeryDAO.GetAllItemCodes();

            List<StationeryViewModel> viewModelList = new List<StationeryViewModel>();
            foreach (Stationery s in itemCode)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        public List<Stationery> GetStationeriesBasedOnCategoryID(int[] categoryID)
        {
            return stationeryDAO.GetStationeriesBasedOnCategoryID(categoryID);
        }

        // TODO - REMOVE THIS METHOD
        public List<string> GetListOfItemCodes()
        {
            return stationeryDAO.GetAllItemCode();
        }
    }
}
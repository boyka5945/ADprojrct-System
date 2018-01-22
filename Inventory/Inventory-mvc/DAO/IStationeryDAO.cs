using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IStationeryDAO
    {
        Stationery FindByItemCode(string itemCode);

        bool AddNewStationery(Stationery stationery);

        int UpdateStationeryInfo(Stationery stationery);

        bool DeleteStationery(string itemCode);

        List<string> GetUOMList();

        List<Category> GetAllCategory();
     
        List<Stationery> GetAllStationery();

        List<string> GetAllItemCode();

        List<String> GetAllUOMList();

        List<string> GetAllFirstSupplierList();

        List<string> GetAllSecondSupplierList();

        List<string> GetAllThirdSupplierList();

        List<int> GetAllCategoryIDList();
        List<Stationery> GetStationeriesBasedOnCriteria(string searchString, string categoryID);
        List<Stationery> GetStationeriesBasedOnCriteria(string itemCodeOrDescription);
        List<Stationery> GetAllItemCodes();
        List<Stationery> GetStationeriesBasedOnCategoryID(int[] categoryID);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface ISupplierDAO
    {
        List<Supplier> GetAllSupplier();

        Supplier FindBySupplierCode(string supplierCode);

        List<string> GetAllSupplierCode();

        bool AddNewSupplier(Supplier supplier);

        int UpdateSupplierInfo(Supplier supplier);

        bool DeleteSupplier(string supplierCode);

        List<Supplier> FindSuppliersForStationery(Stationery s);
    }
}
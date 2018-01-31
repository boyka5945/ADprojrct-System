using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public interface ISupplierService
    {
        List<SupplierViewModel> GetAllSuppliers();

        bool AddNewSupplier(SupplierViewModel supplierVM);

        bool DeleteSupplier(string supplierCode);

        SupplierViewModel FindBySupplierCode(string supplierCode);

        bool UpdateSupplierInfo(SupplierViewModel supplierVM);

        List<Supplier> FindSuppliersForStationery(Stationery s);


        /// <summary>
        /// Return true if the code has already been used
        /// </summary>
        /// <param name="supplierCode"></param>
        /// <returns></returns>
        bool isExistingCode(string supplierCode);

       List<Supplier> GetSupplierList();

    }
}
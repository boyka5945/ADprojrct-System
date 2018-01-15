using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface ISupplierDAO
    {
        List<SupplierDAO> GetAllSupplier();
        User FindBySupplierCode(string supplierCode);
        Boolean AddNewSupplier(SupplierDAO supplier);
        int UpdateSupplierInfo(SupplierDAO supplier);
        Boolean DeleteSupplier(string supplierCode);
    }
}
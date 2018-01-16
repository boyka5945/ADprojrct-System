using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class SupplierService : ISupplierService
    {
        private ISupplierDAO supplierDAO = new SupplierDAO();

        List<Supplier> ISupplierService.GetAllSuppliers()
        {
            return supplierDAO.GetAllSupplier();
        }

        Supplier ISupplierService.GetSupplierByCode(string supplierCode)
        {
            return supplierDAO.FindBySupplierCode(supplierCode);
        }
    }
}
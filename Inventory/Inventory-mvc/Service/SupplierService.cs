using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class SupplierService : ISupplierService
    {
        private ISupplierDAO supplierDAO = new SupplierDAO();

        public List<Supplier> GetSupplierList()
        {
            return supplierDAO.GetAllSupplier();
        }



        public List<SupplierViewModel> GetAllSuppliers()
        {
            List<Supplier> supplierList = supplierDAO.GetAllSupplier();

            List<SupplierViewModel> viewModelList = new List<SupplierViewModel>();
            foreach(Supplier s in supplierList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        public bool isExistingCode(string supplierCode)
        {
            string code = supplierCode.ToUpper().Trim();

            return supplierDAO.GetAllSupplierCode().Contains(code);
        }

        public bool AddNewSupplier(SupplierViewModel supplierVM)
        {
            return supplierDAO.AddNewSupplier(ConvertFromViewModel(supplierVM));
        }

        public bool DeleteSupplier(string supplierCode)
        {
            string code = supplierCode.ToUpper().Trim();
            Supplier supplier = supplierDAO.FindBySupplierCode(code);

            if(supplier.Stationeries.Count != 0 || supplier.Stationeries1.Count != 0 || supplier.Stationeries2.Count != 0)
            {
                return false;
            }

            return supplierDAO.DeleteSupplier(code);
        }

        public SupplierViewModel FindBySupplierCode(string supplierCode)
        {
            string code = supplierCode.ToUpper().Trim();
            return ConvertToViewModel(supplierDAO.FindBySupplierCode(code));
        }

        public bool UpdateSupplierInfo(SupplierViewModel supplierVM)
        {
            Supplier supplier = ConvertFromViewModel(supplierVM);

            if(supplierDAO.UpdateSupplierInfo(supplier) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private SupplierViewModel ConvertToViewModel(Supplier supplier)
        {
            SupplierViewModel supplierVM = new SupplierViewModel();

            supplierVM.SupplierCode = supplier.supplierCode;
            supplierVM.GSTNo = supplier.GSTNo;
            supplierVM.SupplierName = supplier.supplierName;
            supplierVM.ContactName = supplier.contactName;
            supplierVM.PhoneNo = supplier.phoneNo;
            supplierVM.FaxNo = supplier.faxNo;
            supplierVM.Address = supplier.address;

            return supplierVM;
        }

        private Supplier ConvertFromViewModel(SupplierViewModel supplierVM)
        {
            Supplier supplier = new Supplier();

            supplier.supplierCode = supplierVM.SupplierCode;
            supplier.GSTNo = supplierVM.GSTNo;
            supplier.supplierName = supplierVM.SupplierName;
            supplier.contactName = supplierVM.ContactName;
            supplier.phoneNo = (int)supplierVM.PhoneNo;
            supplier.faxNo = supplierVM.FaxNo;
            supplier.address = supplierVM.Address;

            return supplier;
        }

        public List<Supplier> FindSuppliersForStationery(Stationery s)
        {

            return supplierDAO.FindSuppliersForStationery(s);
        }
    }
}
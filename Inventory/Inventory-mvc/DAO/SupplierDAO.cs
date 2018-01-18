using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class SupplierDAO : ISupplierDAO
    {
        bool ISupplierDAO.AddNewSupplier(Supplier supplier)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Suppliers.Add(supplier);
                int rowAffected = context.SaveChanges();

                if (rowAffected != 1)
                {
                    throw new DAOException();
                }

                return true;
            }
        }

        bool ISupplierDAO.DeleteSupplier(string supplierCode)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Supplier supplier = (from s in context.Suppliers
                                     where s.supplierCode == supplierCode
                                     select s).FirstOrDefault();

                context.Suppliers.Remove(supplier);
                context.SaveChanges();

                return true;
            }
        }

        Supplier ISupplierDAO.FindBySupplierCode(string supplierCode)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Suppliers
                        where s.supplierCode == supplierCode
                        select s)
                        .Include(s => s.Stationeries)
                        .Include(s => s.Stationeries1)
                        .Include(s => s.Stationeries2)
                        .FirstOrDefault();
            }
        }

        List<Supplier> ISupplierDAO.GetAllSupplier()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Suppliers
                        select s).ToList();
            }
        }

        List<string> ISupplierDAO.GetAllSupplierCode()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Suppliers
                        select s.supplierCode).ToList();
            }
        }

        int ISupplierDAO.UpdateSupplierInfo(Supplier supplier)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Supplier s = (from x in context.Suppliers
                              where x.supplierCode == supplier.supplierCode
                              select x).FirstOrDefault();

                s.GSTNo = supplier.GSTNo;
                s.supplierName = supplier.supplierName;
                s.contactName = supplier.contactName;
                s.phoneNo = supplier.phoneNo;
                s.faxNo = supplier.faxNo;
                s.address = supplier.address;

                int rowAffected = context.SaveChanges();

                return rowAffected;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class SupplierDAO : ISupplierDAO
    {
        bool ISupplierDAO.AddNewSupplier(Supplier supplier)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Supplier.Add(supplier);
                int rowAffected = context.SaveChanges();

                if (rowAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        bool ISupplierDAO.DeleteSupplier(string supplierCode)
        {
            string code = supplierCode.ToUpper().Trim();

            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    Supplier supplier = (from s in context.Supplier where s.supplierCode == code select s).FirstOrDefault();

                    context.Supplier.Remove(supplier);
                    int rowAffected = context.SaveChanges();

                    if (rowAffected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw new DAOException("Incorrect Supplier Code");
                }
            }
        }

        Supplier ISupplierDAO.FindBySupplierCode(string supplierCode)
        {
            string code = supplierCode.ToUpper().Trim();

            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Supplier where s.supplierCode == code select s).FirstOrDefault();
            }
        }

        List<Supplier> ISupplierDAO.GetAllSupplier()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Supplier select s).ToList();
            }
        }

        int ISupplierDAO.UpdateSupplierInfo(Supplier supplier)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    Supplier s = (from x in context.Supplier where x.supplierCode == supplier.supplierCode select x).FirstOrDefault();

                    s = supplier;

                    int rowAffected = context.SaveChanges();

                    return rowAffected;
                }
                catch (Exception)
                {
                    throw new DAOException("Incorrect Supplier Code");
                }
            }
        }
    }
}
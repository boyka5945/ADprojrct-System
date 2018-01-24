using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class StationeryDAO : IStationeryDAO
    {
        public bool AddNewStationery(Stationery stationery)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Stationeries.Add(stationery);
                int rowAffected = context.SaveChanges();

                if (rowAffected != 1)
                {
                    throw new DAOException();
                }

                return true;
            }
        }


        public bool DeleteStationery(string itemCode)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Stationery stationery = (from s in context.Stationeries
                                     where s.itemCode == itemCode
                                     select s).FirstOrDefault();

                context.Stationeries.Remove(stationery);
                context.SaveChanges();

                return true;
            }
        }

        public Stationery FindByItemCode(string itemCode)
        {
            StationeryModel context = new StationeryModel();

            return (from s in context.Stationeries
                    where s.itemCode == itemCode
                    select s).FirstOrDefault();
        }



        public int UpdateStationeryInfo(Stationery stationery)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Stationery s = (from x in context.Stationeries
                              where x.itemCode == stationery.itemCode
                              select x).FirstOrDefault();

                s.categoryID = stationery.categoryID;
                s.description = stationery.description;
                s.reorderLevel = stationery.reorderLevel;
                s.reorderQty = stationery.reorderQty;
                s.unitOfMeasure = stationery.unitOfMeasure;
                s.stockQty = stationery.stockQty;
                s.location = stationery.location;
                s.firstSupplierCode = stationery.firstSupplierCode;
                s.secondSupplierCode = stationery.secondSupplierCode;
                s.thirdSupplierCode = stationery.thirdSupplierCode;
                s.price = stationery.price;
                int rowAffected = context.SaveChanges();

                return rowAffected;
            }
        }


        public List<string> GetAllItemCode()
        {

            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.itemCode).ToList();
            }
        }



        public List<Stationery> GetAllStationery()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s)
                        .Include(s => s.Category)
                        .ToList();
            }
        }




        public List<string> GetUOMList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.unitOfMeasure).Distinct().ToList();
            }
        }


        public List<Category> GetAllCategory()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from c in context.Categories
                        select c).ToList();
            }

        }




        public List<String> GetAllUOMList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.unitOfMeasure).Distinct().ToList();
            }
        }

        public List<string> GetAllFirstSupplierList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.firstSupplierCode).Distinct().ToList();
            }
        }

        public List<string> GetAllSecondSupplierList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.secondSupplierCode).Distinct().ToList();
            }
        }

        public List<string> GetAllThirdSupplierList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.thirdSupplierCode).Distinct().ToList();
            }
        }

        public List<int> GetAllCategoryIDList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s.categoryID).Distinct().ToList();
            }
        }


        public List<Stationery> GetStationeriesBasedOnCriteria(string searchString, string categoryID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var stationeries = from s in context.Stationeries select s;

                if(categoryID != "-1")
                {
                    stationeries = stationeries.Where(s => s.categoryID.ToString() == categoryID);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    string[] searchStringArray = searchString.Split();
                    foreach (string s in searchStringArray)
                    {
                        string search = s.ToLower().Trim();
                        if (!String.IsNullOrEmpty(search))
                        {
                            stationeries = stationeries.Where(x => x.description.ToLower().Contains(search));
                        }
                    }
                }

                return stationeries.Include(s => s.Category).ToList();
            }

        }

        public List<Stationery> GetStationeriesBasedOnCriteria(string itemCodeOrDescription)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var stationeries = from s in context.Stationeries select s;

                if (!String.IsNullOrEmpty(itemCodeOrDescription))
                {
                    string[] searchStringArray = itemCodeOrDescription.Split();
                    foreach (string s in searchStringArray)
                    {
                        string search = s.ToLower().Trim();
                        if (!String.IsNullOrEmpty(search))
                        {
                            stationeries = stationeries.Where(x => (x.description.ToLower().Contains(search) || x.itemCode.ToLower().Contains(search)));
                        }
                    }
                }

                return stationeries.ToList();
            }

        }

        List<Stationery> IStationeryDAO.GetAllItemCodes()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationeries
                        select s).Include(s => s.Category).ToList();
            }
        }

        public List<Stationery> GetStationeriesBasedOnCategoryID(int[] categoryID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                List<Stationery> stationeries = (from s in context.Stationeries
                                                 where categoryID.Contains(s.categoryID)
                                                 select s).Include(s => s.Category).ToList();

                return stationeries;
            }
        }
    }
}
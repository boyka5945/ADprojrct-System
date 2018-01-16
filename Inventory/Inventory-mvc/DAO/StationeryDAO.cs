using Inventory_mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.DAO
{
    public class StationeryDAO : IStationeryDAO
    {

        bool IStationeryDAO.DeleteStationery(string itemCode)
        {
            throw new NotImplementedException();
        }

        Stationery IStationeryDAO.FindByItemCode(string itemCode)
        {
            StationeryModel context = new StationeryModel();
            return (from s in context.Stationery
                    where s.itemCode == itemCode
                    select s).FirstOrDefault();
        }

       List<Stationery> IStationeryDAO.GetAllStationery()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationery
                        select s).ToList();
            }
        }

        List<string> IStationeryDAO.GetAllStationeryCode()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationery
                        select s.itemCode).ToList();
            }
        }

        int IStationeryDAO.UpdateStationeryInfo(Stationery stationery)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Stationery s = (from x in context.Stationery
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

        bool IStationeryDAO.AddNewStationery(Stationery stationery)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Stationery.Add(stationery);
                int rowAffected = context.SaveChanges();

                if (rowAffected != 1)
                {
                    throw new DAOException();
                }

                return true;
            }
        }
    }
}
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
        bool IStationeryDAO.AddNewStationery(Stationery stationery)
        {
            throw new NotImplementedException();
        }

        bool IStationeryDAO.DeleteStationery(string itemCode)
        {
            throw new NotImplementedException();
        }

        Stationery IStationeryDAO.FindByItemCode(string itemCode)
        {
            throw new NotImplementedException();
        }

        List<Stationery> IStationeryDAO.GetAllStationery()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationery
                        select s)
                        .Include(s => s.Category)
                        .ToList();
            }
        }

        int IStationeryDAO.UpdateStationeryInfo(Stationery stationery)
        {
            throw new NotImplementedException();
        }


        List<string> IStationeryDAO.GetUOMList()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationery
                        select s.unitOfMeasure).Distinct().ToList();
            }
        }


        List<Category> IStationeryDAO.GetAllCategory()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from c in context.Category
                        select c).ToList();
            }

        }

    }
}
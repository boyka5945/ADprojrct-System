using Inventory_mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        User IStationeryDAO.FindByItemCode(string itemCode)
        {
            throw new NotImplementedException();
        }

        //public bool AddNewStationery(StationeryDAO stationery)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool DeleteStationery(string itemCode)
        //{
        //    throw new NotImplementedException();
        //}

        //public User FindByItemCode(string itemCode)
        //{
        //    throw new NotImplementedException();
        //}

        List<Stationery> IStationeryDAO.GetAllStationery()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Stationery
                        select s).ToList();
            }
        }

        int IStationeryDAO.UpdateStationeryInfo(Stationery stationery)
        {
            throw new NotImplementedException();
        }

        //public int UpdateStationeryInfo(StationeryDAO stationery)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IStationeryDAO.AddNewStationery(Stationery stationery)
        //{
        //    using (StationeryModel context = new StationeryModel())
        //    {
        //        context.Stationery.Add(stationery);
        //        int rowAffected = context.SaveChanges();

        //        if (rowAffected != 1)
        //        {
        //            throw new DAOException();
        //        }

        //        return true;
        //    }
        //}
    }
}
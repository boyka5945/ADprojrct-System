﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class CollectionPointDAO : ICollectionPointDAO
    {
        public Collection_Point FindByCollectionPointID(int id)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                Collection_Point cp = entity.Collection_Point.Where(x => x.collectionPointID == id).First();
                return cp;
                

            }


        }

        List<Collection_Point> ICollectionPointDAO.GetAllCollectionPoint()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Collection_Point
                        select s).ToList();
            }
        }

        List<int> ICollectionPointDAO.GetAllCollectionID()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from c in context.Collection_Point
                        select c.collectionPointID).ToList();
            }
        }

        bool ICollectionPointDAO.AddNewCollectionPoint(Collection_Point collectionPoint)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Collection_Point.Add(collectionPoint);
                int rowAffected = context.SaveChanges();

                if (rowAffected != 1)
                {
                    throw new DAOException();
                }

                return true;
            }
        }

        int ICollectionPointDAO.UpdateCollectionPointInfo(Collection_Point collectionPoint)
        {
            using (StationeryModel context = new StationeryModel())
            {
                Collection_Point c = (from x in context.Collection_Point
                                      where x.collectionPointID == collectionPoint.collectionPointID
                                      select x).FirstOrDefault();

                c.collectionPointName = collectionPoint.collectionPointName;


                int rowAffected = context.SaveChanges();

                return rowAffected;
            }

        }
            bool ICollectionPointDAO.DeleteCollectionPoint(int collectionPointID)
        {
                using (StationeryModel context = new StationeryModel())
                {
                    Collection_Point collectionPoint = (from s in context.Collection_Point
                                         where s.collectionPointID == collectionPointID
                                         select s).FirstOrDefault();

                    context.Collection_Point.Remove(collectionPoint);
                    context.SaveChanges();

                    return true;
                }
            }
        public User FindByUserID(string id)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User u = entity.User.Where(x => x.userID == id).Include(x => x.Department2).First();
                return u;


            }


        }
    }



    }

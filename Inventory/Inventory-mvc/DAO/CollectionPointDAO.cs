using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

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

        public List<Collection_Point> GetAllCollectionPoints()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                List<Collection_Point> listAll = entity.Collection_Point.ToList<Collection_Point>();
                return listAll;
            }
        }
    }
}
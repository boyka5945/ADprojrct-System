using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public class CollectionPointService : ICollectionPointService
    {
        public List<Collection_Point> GetAllCollectionPoint()
        {
            CollectionPointDAO cDAO = new CollectionPointDAO();
            return cDAO.GetAllCollectionPoints();
        }

        public Collection_Point GetCollectionPointByID(int collectionPointID)
        {
            CollectionPointDAO cDAO = new CollectionPointDAO();
            return cDAO.FindByCollectionPointID(collectionPointID);
        }
    }
}
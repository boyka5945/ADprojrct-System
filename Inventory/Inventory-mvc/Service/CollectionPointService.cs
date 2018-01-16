using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class CollectionPointService : ICollectionPointService
    {
        private ICollectionPointDAO collectionPointDAO = new CollectionPointDAO();

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

        bool ICollectionPointService.isExistingCode(int collectionPointID)
        {
            int Id = collectionPointID;

            return collectionPointDAO.GetAllCollectionID().Contains(Id);
        }

        bool ICollectionPointService.AddNewCollectionPoint(CollectionPointViewModel cpVM)
        {
            return collectionPointDAO.AddNewCollectionPoint(ConvertFromViewModel(cpVM));
        }

        private CollectionPointViewModel ConvertToViewModel(Collection_Point collectionPoint)
        {
            CollectionPointViewModel cpVM = new CollectionPointViewModel();

            cpVM.collectionPointID = collectionPoint.collectionPointID;
            cpVM.collectionPointName = collectionPoint.collectionPointName;
           

            return cpVM;
        }

        private Collection_Point ConvertFromViewModel(CollectionPointViewModel cpVM)
        {
            Collection_Point collectionPoint = new Collection_Point();

            collectionPoint.collectionPointID = cpVM.collectionPointID;
            collectionPoint.collectionPointName = cpVM.collectionPointName;
            

            return collectionPoint;
        }
    }
}
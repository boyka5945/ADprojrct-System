using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class CollectionPointService : ICollectionPointService
    {
        private ICollectionPointDAO collectionPointDAO = new CollectionPointDAO();

        //public List<Collection_Point> GetAllCollectionPoint()
        //{
        //    CollectionPointDAO cDAO = new CollectionPointDAO();
        //    return cDAO.GetAllCollectionPoints();
        //}

        List<CollectionPointViewModel> ICollectionPointService.GetAllCollectionPoints()
        {
            List<Collection_Point> collectionPointList = collectionPointDAO.GetAllCollectionPoint();

            List<CollectionPointViewModel> viewModelList = new List<CollectionPointViewModel>();
            foreach (Collection_Point s in collectionPointList)
            {
                viewModelList.Add(ConvertToViewModel(s));
            }

            return viewModelList;
        }

        public CollectionPointViewModel GetCollectionPointByID(int collectionPointID)
        {
            int id = collectionPointID;
            return ConvertToViewModel(collectionPointDAO.FindByCollectionPointID(id));
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

        bool ICollectionPointService.UpdateCollectionPointInfo(CollectionPointViewModel cpVM)
        {
            Collection_Point collectionPoint = ConvertFromViewModel(cpVM);

            if (collectionPointDAO.UpdateCollectionPointInfo(collectionPoint) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollectionPointService.DeleteCollectionPoint(int collectionPointID)
        {
            int code = collectionPointID;
            Collection_Point collectionPoint = collectionPointDAO.FindByCollectionPointID(code);

            //if (collectionPoint.Department.Count != 0)
            //{
            //    return false;
            //}

            return collectionPointDAO.DeleteCollectionPoint(code);
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
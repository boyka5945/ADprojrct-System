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

        List<Collection_Point> ICollectionPointService.GetAllCollectionPoints2()
        {
           
            return collectionPointDAO.GetAllCollectionPoint();
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

        public UserViewModel FindByUserID(string userID)
        {
            string id = userID;
            return ConvertToViewModel2(collectionPointDAO.FindByUserID(id));
        }

        private UserViewModel ConvertToViewModel2(User user)
        {
            UserViewModel uVM = new UserViewModel();

            uVM.DepartmentName = user.Department.departmentName;

            uVM.UserID = user.userID;
            uVM.Password = user.password;
            uVM.Name = user.name;
            uVM.Address = user.address;
            uVM.ContactNo = user.contactNo;
            uVM.UserEmail = user.userEmail;
            uVM.DepartmentCode = user.departmentCode;
            uVM.Role = user.role;
            uVM.DelegationStart =(DateTime)user.delegationStart;
            uVM.DelegationEnd = (DateTime)user.delegationEnd;


            return uVM;
        }
    }
}
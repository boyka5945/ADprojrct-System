using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    interface ICollectionPointService
    {
        bool isExistingCode(int collectionPointID);

        List<CollectionPointViewModel> GetAllCollectionPoints();

        CollectionPointViewModel GetCollectionPointByID(int collectionPointID);

       bool AddNewCollectionPoint(CollectionPointViewModel cpVM);

        bool UpdateCollectionPointInfo(CollectionPointViewModel cpVM);

        bool DeleteCollectionPoint(int collectionPointID);

        UserViewModel FindByUserID(string userID);

    }
}

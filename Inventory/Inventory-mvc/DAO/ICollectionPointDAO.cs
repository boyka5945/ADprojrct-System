using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    interface ICollectionPointDAO
    {
        List<Collection_Point> GetAllCollectionPoint();

        
        Collection_Point FindByCollectionPointID(int id);

        List<int> GetAllCollectionID();

        bool AddNewCollectionPoint(Collection_Point collectionPoint);

        bool DeleteCollectionPoint(int collectionPointID);

        int UpdateCollectionPointInfo(Collection_Point collectionPoint);

        User FindByUserID(string id);
    }
}

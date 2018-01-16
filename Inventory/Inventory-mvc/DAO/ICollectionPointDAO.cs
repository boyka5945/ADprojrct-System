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
        List<Collection_Point> GetAllCollectionPoints();

        Collection_Point FindByCollectionPointID(int id);
    }
}

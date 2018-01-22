using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public interface IInventoryStatusRecordService
    {
        List<InventoryCheckViewModel> GetInventoryChecklistBasedOnCategory(int[] categoryID);
    }
}
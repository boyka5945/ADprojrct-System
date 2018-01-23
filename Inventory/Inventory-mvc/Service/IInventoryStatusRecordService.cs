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

        List<InventoryCheckViewModel> ConvertStockChecklistToDiscrepancyList(List<InventoryCheckViewModel> stockchecklist);

        bool SaveInventoryCheckResult(List<InventoryCheckViewModel> stockchecklist);

        bool SubmitAdjustmentVoucherForInventoryCheckDiscrepancy(List<InventoryCheckViewModel> stockchecklist, string requesterID);

        List<DateTime> ListAllStockCheckDate();

        List<InventoryCheckViewModel> FindInventoryStatusRecordsByDate(DateTime date);

       bool IsStockCheckConductedForCategoriesOnDate(DateTime date, int[] categoryID, out List<string> checkedCategories);
    }
}
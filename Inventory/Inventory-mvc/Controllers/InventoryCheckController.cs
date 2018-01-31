using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;
using System.Transactions;
using Rotativa.MVC;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class InventoryCheckController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();
        IInventoryStatusRecordService invetoryCheckService = new InventoryStatusRecordService();

        [RoleAuthorize]
        // CK - Store Clerk | Store Supervisor | Store Manager
        public ActionResult Index()
        {
            List<DateTime> dates = invetoryCheckService.ListAllStockCheckDate();
            ViewBag.Dates = dates;

            return View();
        }

        [RoleAuthorize]
        // CK - Store Clerk | Store Supervisor | Store Manager
        public ActionResult ShowDetails(DateTime? date = null)
        {
            if (date == null)
            {
                return RedirectToAction("Index");
            }

            List<InventoryCheckViewModel> vmList = invetoryCheckService.FindInventoryStatusRecordsByDate((DateTime) date);

            if(vmList.Count == 0)
            {
                TempData["ErrorMessage"] = "Non-existing inventory check record.";
            }

            return View(vmList);
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpGet]
        public ActionResult GenerateInventoryChecklist()
        {
            if (HttpContext.Application["InventoryChecklist"] != null)
            {
                // must complete current stock check first before generate new one
                TempData["ErrorMessage"] = String.Format("Please complete or cancel current stock check before generate another checklist.");
                return RedirectToAction("ProcessInventoryCheck");
            }

            ViewBag.CategoryList = stationeryService.GetAllCategory();

            List<Category> categories = stationeryService.GetAllCategory();
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach(Category c in categories)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = c.categoryName,
                    Value = c.categoryID.ToString()
                };

                selectList.Add(item);
            }

            ViewBag.SelectList = selectList;


            return View();
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpPost]
        public ActionResult GenerateInventoryChecklist(int[] categorylistbox = null)
        {

            string errorMessage = null;
            List<string> checkedCategories;

            if(categorylistbox == null)
            {
                errorMessage = "Please select at least one category.";
            }
            else if(adjustmentVoucherService.GetPendingVoucherCount() != 0)
            {
                // check whether has pending adjustment voucher
                errorMessage = String.Format("There are {0} pending adjustment voucher(s). Please process all vouchers first before generate the list.", adjustmentVoucherService.GetPendingVoucherCount());
            }
            else if(invetoryCheckService.IsStockCheckConductedForCategoriesOnDate(DateTime.Today, categorylistbox, out checkedCategories))
            {
                errorMessage = String.Format("Stock check results for categories: {0} has been submitted today.", String.Join(", ", checkedCategories));
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("GenerateInventoryChecklist");
            }

            List<InventoryCheckViewModel> vmList = invetoryCheckService.GetInventoryChecklistBasedOnCategory(categorylistbox);

            if(vmList.Count == 0)
            {
                TempData["ErrorMessage"] = "There is no stationery for selected categories.";
                return RedirectToAction("GenerateInventoryChecklist");
            }

            HttpContext.Application.Lock();
            HttpContext.Application["InventoryChecklist"] = vmList;
            HttpContext.Application.UnLock();

            return RedirectToAction("ProcessInventoryCheck", vmList);
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpGet]
        public ActionResult ProcessInventoryCheck(int? page)
        {
            HttpContext.Application.Lock();
            List<InventoryCheckViewModel> stockchecklist = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
            HttpContext.Application.UnLock();

            if (stockchecklist == null) // not yet generate any list
            {
                return RedirectToAction("GenerateInventoryChecklist");
            }

            ViewBag.Page = page;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(stockchecklist.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpGet]
        public ActionResult GenerateStockCheckList()
        {
            HttpContext.Application.Lock();
            List<InventoryCheckViewModel> stockchecklist = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
            HttpContext.Application.UnLock();

            if (stockchecklist == null) // not yet generate any list
            {
                return RedirectToAction("GenerateInventoryChecklist");
            }

            string fileName = String.Format("Inventory_Stock_Checklist_on_{0}.pdf", stockchecklist.First().StockCheckDate.ToLongDateString());
            return new ViewAsPdf("_StockCheckListPDF", stockchecklist) { FileName = fileName };
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpPost]
        public void SaveTemporaryValue(List<InventoryCheckViewModel> checklist)
        {
            if(checklist != null || checklist.Count != 0)
            {
                HttpContext.Application.Lock();
                List<InventoryCheckViewModel> stockchecklist = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
                HttpContext.Application.UnLock();

                foreach(InventoryCheckViewModel item in checklist)
                {
                    InventoryCheckViewModel vm = stockchecklist.Find(x => x.ItemCode == item.ItemCode);
                    vm.ActualQuantity = item.ActualQuantity;
                }

                HttpContext.Application.Lock();
                HttpContext.Application["InventoryChecklist"] = stockchecklist;
                HttpContext.Application.UnLock();
            }

        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpGet]
        public ActionResult CancelCurrentStockCheck()
        {
            HttpContext.Application.Lock();
            HttpContext.Application["InventoryChecklist"] = null;
            HttpContext.Application.UnLock();

            TempData["SuccessMessage"] = "Stock check has been cancelled.";
            return RedirectToAction("GenerateInventoryChecklist");
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpPost]
        public ActionResult SubmitInventoryCheckResult()
        {
            HttpContext.Application.Lock();
            List<InventoryCheckViewModel> stockchecklist = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
            HttpContext.Application.UnLock();

            // find discrepancy
            List<InventoryCheckViewModel> discrepancylist = invetoryCheckService.ConvertStockChecklistToDiscrepancyList(stockchecklist);

            if(discrepancylist.Count() == 0) // if not disprecancy then save into database directly
            {
                try
                {
                    invetoryCheckService.SaveInventoryCheckResult(stockchecklist);

                    // clear list
                    HttpContext.Application.Lock();
                    HttpContext.Application["InventoryChecklist"] = null;
                    HttpContext.Application.UnLock();

                    TempData["SuccessMessage"] = "Stock check result has been submitted.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return RedirectToAction("ProcessInventoryCheck");
                }
            }
            else
            {
                // show discrepany report
                return View(discrepancylist);
            }
        }

        [RoleAuthorize]
        // CK - Store Clerk
        [HttpPost]
        public ActionResult ConfirmInventoryCheckResult(List<InventoryCheckViewModel> discrepancylist)
        {
            HttpContext.Application.Lock();
            List<InventoryCheckViewModel> stockchecklist = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
            HttpContext.Application.UnLock();

            foreach(var item in discrepancylist)
            {
                InventoryCheckViewModel vm = stockchecklist.Find(x => x.ItemCode == item.ItemCode);
                vm.Remarks = item.Remarks;
            }

            HttpContext.Application.Lock();
            HttpContext.Application["InventoryChecklist"] = stockchecklist;
            HttpContext.Application.UnLock();

            string requesterID = HttpContext.User.Identity.Name;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    try
                    {
                        invetoryCheckService.SubmitAdjustmentVoucherForInventoryCheckDiscrepancy(stockchecklist, requesterID);
                    }
                    catch (EmailException e)
                    {
                        TempData["WarningMessage"] = "Failure to send email notification. Kindly contact IT personnel.";
                    }

                    invetoryCheckService.SaveInventoryCheckResult(stockchecklist);

                    HttpContext.Application.Lock();
                    HttpContext.Application["InventoryChecklist"] = null;
                    HttpContext.Application.UnLock();

                    ts.Complete();

                    TempData["SuccessMessage"] = "Stock check result has been submitted.";
                    return RedirectToAction("Index");
                }
                catch (Exception e1)
                {
                    TempData["ErrorMessage"] = "Error when submitting adjustment voucher";
                }
            }

            return RedirectToAction("ProcessInventoryCheck");
        }

    }
}
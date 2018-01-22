using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class InventoryCheckController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();
        IInventoryStatusRecordService invetoryCheckService = new InventoryStatusRecordService();

        // GET: InventoryCheck
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateInventoryChecklist()
        {
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


        [HttpPost]
        public ActionResult GenerateInventoryChecklist(int[] categorylistbox = null)
        {
            string errorMessage = null;

            if(categorylistbox == null)
            {
                errorMessage = "Please select at least one category.";
            }
            else if(adjustmentVoucherService.GetPendingVoucherCount() != 0)
            {
                // check whether has pending adjustment voucher
                errorMessage = String.Format("There is {0} pending adjustment voucher(s). Please process all vouchers first before generate the list.", adjustmentVoucherService.GetPendingVoucherCount());
            }

            if(!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("GenerateInventoryChecklist");
            }

            List<InventoryCheckViewModel> vmList = invetoryCheckService.GetInventoryChecklistBasedOnCategory(categorylistbox);
            HttpContext.Application.Lock();
            HttpContext.Application["InventoryChecklist"] = vmList;
            HttpContext.Application.UnLock();


            return RedirectToAction("ProcessInventoryCheck", vmList);
        }


        [HttpGet]
        public ActionResult ProcessInventoryCheck(List<InventoryCheckViewModel> checklist, int? page)
        {
            HttpContext.Application.Lock();
            List<InventoryCheckViewModel> currentList = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
            HttpContext.Application.UnLock();
            if (currentList == null) // not yet generate any list
            {
                return RedirectToAction("GenerateInventoryChecklist");
            }

            if(checklist == null) // from GenerateInventoryChecklist page
            {
                checklist = currentList;
            }
            else
            {
                // from ProcessInventoryCheck after change page
                // store input quantity
                HttpContext.Application.Lock();
                HttpContext.Application["InventoryChecklist"] = checklist;
                HttpContext.Application.UnLock();
            }

            ViewBag.Page = page;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(checklist.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SubmitInventoryCheckResult()
        {
            return View();
        }

        [HttpPost]
        public void SaveTemporaryValue(List<InventoryCheckViewModel> checklist)
        {
            if(checklist != null || checklist.Count != 0)
            {
                HttpContext.Application.Lock();
                List<InventoryCheckViewModel> currentList = HttpContext.Application["InventoryChecklist"] as List<InventoryCheckViewModel>;
                HttpContext.Application.UnLock();

                foreach(InventoryCheckViewModel item in checklist)
                {
                    InventoryCheckViewModel vm = currentList.Find(x => x.ItemCode == item.ItemCode);
                    vm.ActualQuantity = item.ActualQuantity;
                }

                HttpContext.Application.Lock();
                HttpContext.Application["InventoryChecklist"] = currentList;
                HttpContext.Application.UnLock();
            }

        }

    }
}
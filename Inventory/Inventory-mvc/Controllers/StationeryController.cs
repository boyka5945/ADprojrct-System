using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class StationeryController : Controller
    {

        IStationeryService stationeryService = new StationeryService();
        ISupplierService supplierService = new SupplierService();
        IUserService userService = new UserService();
        ITransactionRecordService transactionService = new TransactionRecordService();

        [RoleAuthorize]
        // Manager, Store Clerk, Store Supervisor
        // GET: Stationery
        public ActionResult Index(string searchString, int? page, string categoryID = "-1")
        {
            string userID = HttpContext.User.Identity.Name;

            // Store clerk roleID == 7
            ViewBag.RoleID = userService.GetRoleByID(userID);


            List<StationeryViewModel> stationeries = stationeryService.GetStationeriesVMBasedOnCriteria(searchString, categoryID);

            ViewBag.CategoryList = stationeryService.GetAllCategory();
            ViewBag.CategoryID = (categoryID == "-1") ? "-1" : categoryID;
            ViewBag.SearchString = searchString;
            ViewBag.Page = page;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }


        [RoleAuthorize]
        // GET: Supplier/Edit/{id}
        //Store Manager, Store Supervisor
        public ActionResult Edit(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            StationeryViewModel stationeryVM = stationeryService.FindStationeryViewModelByItemCode(id);
            return View(stationeryVM);
        }

        [RoleAuthorize]
        // POST: Supplier/Edit/{id}
        //Store Manager, Store Supervisor
        [HttpPost]
        public ActionResult Edit(StationeryViewModel stationeryVM)
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            string code = stationeryVM.ItemCode;
            int level = stationeryVM.ReorderLevel;
            int qty = stationeryVM.ReorderQty;
            decimal price = stationeryVM.Price;

            //for supplier validate
            string supplier1 = stationeryVM.FirstSupplierCode;
            string supplier2 = stationeryVM.SecondSupplierCode;
            string supplier3 = stationeryVM.ThirdSupplierCode;
            if (stationeryService.isExistingSupplierCode(supplier1, supplier2))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("SecondSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier1, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier2, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in Second Supplier.", supplier2);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isPositiveLevel(level))
            {
                string errorMessage = String.Format("{0}  must be positive.", level);
                ModelState.AddModelError("ReorderLevel", errorMessage);
            }
            if (stationeryService.isPositiveQty(qty))
            {
                string errorMessage = String.Format("{0}  must be positive.", qty);
                ModelState.AddModelError("ReorderQty", errorMessage);
            }
            if (stationeryService.isPositivePrice(price))
            {
                string errorMessage = String.Format("{0}  must be positive.", price);
                ModelState.AddModelError("Price", errorMessage);
            }

            else if (ModelState.IsValid)
            {
                try
                {
                    if (stationeryService.UpdateStationeryInfo(stationeryVM))
                    {
                        TempData["SuccessMessage"] = String.Format("'{0}' has been updated", code);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = String.Format("There is not change to '{0}'.", code);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                }
            }
            return View(stationeryVM);
        }

        [RoleAuthorize]
        // GET: Stationery/Create
        //Store Manager, Store Supervisor
        public ActionResult Create()
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            // return View(new StationeryViewModel());
            return View();
        }

        [RoleAuthorize]
        // POST: Stationery/Create
        //Store Manager, Store Supervisor
        [HttpPost]
        public ActionResult Create(StationeryViewModel stationeryVM)
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            string code = stationeryVM.ItemCode;
            int level = stationeryVM.ReorderLevel;
            int qty = stationeryVM.ReorderQty;
            decimal price = stationeryVM.Price;

            //for supplier validate
            string supplier1 = stationeryVM.FirstSupplierCode;
            string supplier2 = stationeryVM.SecondSupplierCode;
            string supplier3 = stationeryVM.ThirdSupplierCode;
            if (stationeryService.isExistingSupplierCode(supplier1,supplier2))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.",supplier1);
                ModelState.AddModelError("SecondSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier1, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier2, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in Second Supplier.", supplier2);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingCode(code))
            {
                string errorMessage = String.Format("{0} has been used.", code);
                ModelState.AddModelError("ItemCode", errorMessage);
            }
            if (stationeryService.isPositiveLevel(level))
            {
                string errorMessage = String.Format("{0}  must be positive.", level);
                ModelState.AddModelError("ReorderLevel", errorMessage);
            }
            if (stationeryService.isPositiveQty(qty))
            {
                string errorMessage = String.Format("{0}  must be positive.", qty);
                ModelState.AddModelError("ReorderQty", errorMessage);
            }
            if (stationeryService.isPositivePrice(price))
            {
                string errorMessage = String.Format("{0}  must be positive.", price);
                ModelState.AddModelError("Price", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                {
                    try
                    {
                        stationeryService.AddNewStationery(stationeryVM);
                        TempData["SuccessMessage"] = String.Format("Stationery '{0}' is added.", code);
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        TempData["ExceptionMessage"] = e.Message;
                    }
                }
            }

            return View(stationeryVM);
        }
        [RoleAuthorize]
        // GET: Stationery/Delete/{id}
        //Store Manager, Store Supervisor
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            if (stationeryService.DeleteStationery(id))
            {
                TempData["SuccessMessage"] = String.Format("Stationery '{0}' has been deleted", id);
            }
            else
            {
                TempData["ErrorMessage"] = String.Format("Cannot delete stationery '{0}'", id);
            }

            return RedirectToAction("Index");
        }

        [RoleAuthorize]
        // GET: Stationery/Details
        //Store Manager, Store Supervisor, Store Clerk
        public ActionResult ViewStockCard(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            string userID = HttpContext.User.Identity.Name;

            // Store clerk roleID == 7
            ViewBag.RoleID = userService.GetRoleByID(userID);


            ViewBag.SelectYear = new SelectList(transactionService.GetSelectableTransactionYear(DateTime.Today.Year));

            ViewBag.SelectMonth = new SelectList(transactionService.GetSelectableTransactionMonth(DateTime.Today.Month));

            return View(stationeryService.FindStationeryViewModelByItemCode(id));
        }

        [RoleAuthorize]
        //Store Manager, Store Supervisor, Store Clerk
        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("Index", new { searchString = "", categoryID = "-1" });
        }

        [RoleAuthorize]
        //Manager
        [HttpPost]
        public ActionResult ViewTransaction(string id, int selectedYear, int selectedMonth)
        {
            List<ItemTransactionRecordViewModel> vmList = transactionService.GetTransaciontDetailsViewModelByCriteria(selectedYear, selectedMonth, id);
            return PartialView("_ViewTransaction", vmList);
        }

    }
}
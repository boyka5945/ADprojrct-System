using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Service;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class RaiseRequisitionController : Controller
    {
        IStationeryService stationeryService = new StationeryService();

        // GET: RaiseRequisition/BrowseCatalogue
        public ActionResult BrowseCatalogue(string searchString, int? page, string categoryID = "All")
        {
            List<Stationery> stationeries = stationeryService.GetAllStationery();
            ViewBag.CategoryList = stationeryService.GetAllCategory();

            if (categoryID == "All")
            {
                ViewBag.CategoryID = "All";
            }
            else
            {
                ViewBag.CategoryID = categoryID;
                stationeries = (from s in stationeries
                                where s.categoryID.ToString() == categoryID
                                select s).ToList();
            }

            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                string[] searchStringArray = searchString.Split();
                foreach (string s in searchStringArray)
                {
                    string search = s.ToLower().Trim();
                    if (!String.IsNullOrEmpty(search))
                    {
                        stationeries = (from x in stationeries
                                        where x.description.ToLower().Contains(search)
                                        select x).ToList();
                    }
                }
            }

            ViewBag.Page = page;

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("BrowseCatalogue", new { searchString = "", categoryID = "All" });
        }

        public ActionResult NewRequisition()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            return View(requestList);
        }

        [HttpGet]
        public ActionResult CreateEditRequestItem(string itemCode)
        {

            // NEW OR EDIT
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            RaiseRequisitionViewModel viewModel = new RaiseRequisitionViewModel();

            if (!String.IsNullOrEmpty(itemCode))
            {
                viewModel = requestList.Find(x => x.ItemCode == itemCode);
            }


            // GET SELECT LIST FOR DROP DOWN BOX
            List<Stationery> stationeries = stationeryService.GetAllStationery();
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (Stationery s in stationeries)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Text = s.description + " (" + s.unitOfMeasure + ")",
                    Value = s.itemCode,
                    Selected = false
                });
            }

            foreach (SelectListItem item in selectListItems)
            {
                if (item.Value == viewModel.ItemCode)
                {
                    item.Selected = true;
                }
            }

            ViewBag.SelectList = selectListItems;

            return PartialView("_CreateEditPartial", viewModel);
        }


        [HttpPost]
        public ActionResult AddNewRequestItem(string itemCode, int quantity, string searchString, int? page, string categoryID)
        {
            // Server side validation
            if(quantity < 1)
            {
                string addItemErrorMessage = String.Format("Quantity must be greater than or equal to 1.");
                TempData["AddItemErrorMessage"] = addItemErrorMessage;
            }
            else
            {
                List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

                Stationery stationery = stationeryService.FindStationeryByItemCode(itemCode);
                RaiseRequisitionViewModel vm = new RaiseRequisitionViewModel();
                vm.Description = stationery.description;
                vm.ItemCode = stationery.itemCode;
                vm.Quantity = quantity;
                vm.UOM = stationery.unitOfMeasure;

                bool contain = false;
                foreach (var request in requestList)
                {
                    if (request.ItemCode == vm.ItemCode)
                    {
                        request.Quantity += vm.Quantity;
                        contain = true;
                        break;
                    }
                }
                if (!contain)
                {
                    requestList.Add(vm);
                }
                Session["RequestList"] = requestList;

                string addItemMessage = String.Format("{0} x {1} was added into requisition form.", vm.Quantity, vm.Description);
                TempData["AddItemMessage"] = addItemMessage;
            }

            ViewBag.SearchString = searchString;
            ViewBag.CategoryID = categoryID;
            ViewBag.Page = page;

            string path = Request.UrlReferrer.ToString();
            if(path.Contains("NewRequisition"))
            {
                return RedirectToAction("NewRequisition");
            }
            else
            {
                return RedirectToAction("BrowseCatalogue",
                        new { searchString = searchString, categoryID = categoryID, page = page });
            }
        }

        [HttpPost]
        public ActionResult EditRequestItem(RaiseRequisitionViewModel model)
        {         
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            RaiseRequisitionViewModel request = requestList.Find(x => x.ItemCode == model.ItemCode);
            request.Quantity = model.Quantity;
            Session["RequestList"] = requestList;

            TempData["EditItemMessage"] = String.Format("Quantity of {0} was updated.", request.Description);

            return RedirectToAction("NewRequisition");
        }

        public ActionResult RemoveRequestItem(string itemCode)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            RaiseRequisitionViewModel vm = requestList.Find(x => x.ItemCode == itemCode);
            string itemDescription = vm.Description;
            requestList.Remove(vm);
            Session["RequestList"] = requestList;

            TempData["RemoveItemMessage"] = String.Format("{0} was removed.", itemDescription);

            return RedirectToAction("NewRequisition");
        }


        public ActionResult ClearAllRequestItem()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            requestList.Clear();
            Session["RequestList"] = requestList;

            TempData["RemoveItemMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewRequisition");
        }

        [HttpPost]
        public ActionResult SubmitRequisition()
        {
            // TODO: Check email notification

            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;


            string requesterID = HttpContext.User.Identity.Name;



            string managerID = null;

            return View();
        }



    }
}
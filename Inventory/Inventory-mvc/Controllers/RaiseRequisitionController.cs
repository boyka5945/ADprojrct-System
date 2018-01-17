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
                string search = searchString.ToLower().Trim();

                stationeries = (from s in stationeries
                                where s.description.ToLower().Contains(search)
                                select s).ToList();
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("BrowseCatalogue", new { searchString = "", categoryID = "All" });
        }

        [HttpGet]
        public ActionResult AddNewRequestItem(string itemCode = null)
        {
            //TODO: Fix modal-backdrop, Fix validation

            List<SelectListItem> stationerySelectItemList = new List<SelectListItem>();
            List<Stationery> stationeries = stationeryService.GetAllStationery();

            foreach(var stationery in stationeries)
            {
                stationerySelectItemList.Add(new SelectListItem()
                {
                    Text = stationery.description,
                    Value = stationery.itemCode,
                    Selected = false
                });
            }

            ViewBag.StationerySelectItemList = stationerySelectItemList;

            RaiseRequisitionViewModel vm = new RaiseRequisitionViewModel();

            if(itemCode != null)
            {
                Stationery stationery = stationeryService.FindStationeryByItemCode(itemCode);

                vm.ItemCode = stationery.itemCode;
                vm.Description = stationery.description;
                vm.Quantity = 1;
                vm.UOM = stationery.unitOfMeasure;
            }

            return PartialView("RequestPartial", vm);
        }



        [HttpPost]
        public string AddNewRequestItem(RaiseRequisitionViewModel vm)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

            bool contain = false;

            foreach(var request in requestList)
            {
                if(request.ItemCode == vm.ItemCode)
                {
                    request.Quantity += vm.Quantity;
                    contain = true;
                    break;
                }
            }

            if(!contain)
            {
                requestList.Add(vm);
            }
                
            Session["RequestList"] = requestList;

            return "OK";
        }

        public ActionResult NewRequisition()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            return View(requestList);
        }

        public ActionResult RemoveRequestItem(int position)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

            string itemDescription = requestList.ElementAt(position).Description;
            
            requestList.RemoveAt(position);
            Session["RequestList"] = requestList;

            TempData["RemoveItemMessage"] = String.Format("{0} was removed.", itemDescription);

            return RedirectToAction("NewRequisition");
        }

        public ActionResult SubmitRequisition()
        {
            // TODO: Send email notification

            return View();
        }


        public ActionResult ClearAllRequestItem()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            requestList.Clear();

            Session["RequestList"] = requestList;

            TempData["RemoveItemMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewRequisition");
        }



    }
}
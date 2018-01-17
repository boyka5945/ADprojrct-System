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
        public ActionResult AddNewRequest(string itemCode)
        {
            RaiseRequisitionViewModel vm = new RaiseRequisitionViewModel();
            Stationery stationery = stationeryService.FindStationeryByItemCode(itemCode);

            vm.ItemCode = stationery.itemCode;
            vm.Description = stationery.description;
            vm.Quantity = 0;
            vm.UOM = stationery.unitOfMeasure;         

            return PartialView("RequestPartial", vm);
        }

    }
}
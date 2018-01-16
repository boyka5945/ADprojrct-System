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

        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("BrowseStationeryCatalogue", new { searchString = "",  categoryID = "All" });
        }

        // GET: RequisitionDetails
        public ActionResult BrowseStationeryCatalogue(string searchString, int? page, string categoryID = "All")
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

    }
}
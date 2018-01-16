using Inventory_mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_mvc.Controllers
{
    public class StationeryController : Controller
    {
        private object stationeryService;
        private object stationeryVM;

        // GET: Stationery
        public ActionResult Index()
        {
            return View();
        }

        // GET: Stationert/Create
        public ActionResult Create()
        {
            return View(new StationeryViewModel());
        }

        // POST: Supplier/Create
        [HttpPost]
        public ActionResult Create(StationeryViewModel supplierVM)
        {
            string code = stationeryVM.ItemCode;

            if (stationeryService.isExistingCode(code))
            {
                string errorMessage = String.Format("{0} has been used.", code);
                ModelState.AddModelError("ItemCode", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    stationeryService.AddNewStationery(stationeryVM);
                    TempData["CreateMessage"] = String.Format("Stationery '{0}' is added.", code);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            return View(supplierVM);
        }
    }
}
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;
using System;
using System.Web.Mvc;

namespace Inventory_mvc.Controllers
{
    public class StationeryController : Controller
    {
        IStationeryService stationeryService = new StationeryService();

        // GET: Stationery
        public ActionResult Index()
        {
            return View();
        }

        // GET: Stationery/Create
        public ActionResult Create()
        {
            return View(new StationeryViewModel());
        }

        // POST: Stationery/Create
        [HttpPost]
        public ActionResult Create(StationeryViewModel stationeryVM)
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

            return View(stationeryVM);
        }
    }
}
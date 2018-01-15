using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_mvc.Controllers
{
    public class StationeryController : Controller
    {
        // GET: Stationery
        public ActionResult Index()
        {
            return View();
        }
    }
}
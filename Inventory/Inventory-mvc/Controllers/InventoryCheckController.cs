using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_mvc.Controllers
{
    public class InventoryCheckController : Controller
    {
        // GET: InventoryCheck
        public ActionResult Index()
        {
            return View();
        }
    }
}
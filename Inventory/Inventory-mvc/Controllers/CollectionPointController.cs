using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class CollectionPointController : Controller
    {
        // GET: CollectionPoint
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListCollectionPoint()
        {
            CollectionPointService ds = new CollectionPointService();
            List<Collection_Point> model = ds.GetAllCollectionPoint();
            return View(model);
        }
    }
}
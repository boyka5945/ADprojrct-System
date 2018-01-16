using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class SupplierController : Controller
    {
        ISupplierService supplierService = new SupplierService();

        // GET: Supplier
        public ActionResult Index()
        {
            ViewBag.Number = 1000;
            return View(supplierService.GetAllSuppliers());
        }

        public ActionResult Create()
        {
            return View(new Supplier());
        }


        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {            
            return View();
        }

        public ActionResult Details(string id)
        {
            return View(supplierService.GetSupplierByCode(id));
        }

    }
}
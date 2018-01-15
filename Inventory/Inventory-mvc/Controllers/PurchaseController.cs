using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: PurchaseDetails

        PurchaseOrderService pos = new PurchaseOrderService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListPurchaseOrders()
        {
            
            List<Purchase_Order_Record> model = pos.GetAllPurchaseOrder();
            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

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

        [HttpGet]
        public ActionResult ListPurchaseOrders(string search)
        {
            List<Purchase_Order_Record> model = new List<Purchase_Order_Record>();

            if (search!=null)
            {

               Purchase_Order_Record por = pos.FindByOrderID(Int32.Parse(search));

                model.Add(por);
            }

            else
            {

                model = pos.GetAllPurchaseOrder();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult RaisePurchaseOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RaisePurchaseOrder([Bind(Include = "orderNo, itemCode, qty, remarks")]Purchase_Details pd)
        {
            using(StationeryModel Entity = new StationeryModel())
            {
                Entity.Purchase_Details.Add(pd);
                Entity.SaveChanges();
            }

            List<Purchase_Order_Record> model = pos.GetAllPurchaseOrder();
            return View("ListPurchaseOrders", model);

            
        }

        public ActionResult DeleteOrder(string id)
        {

            pos.DeletePurchaseOrder(Int32.Parse(id));
            return View("ListPurchaseOrders");

        }
    }
}
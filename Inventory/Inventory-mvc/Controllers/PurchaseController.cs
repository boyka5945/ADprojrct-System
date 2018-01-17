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
        public ActionResult ObtainPurchaseDetails(string id)
        {
            return View();

        }

        [HttpGet]
        public ActionResult RaisePurchaseOrder()
        {
            //to obtain latest order number
            int orderNo = findNextOrderNo();

            ViewBag.orderNo = orderNo;

            List<Purchase_Details> model = new List<Purchase_Details>();

            if (Session["purchaseOrder"] != null)
            {
                model = (List<Purchase_Details>)Session["purchaseOrder"];
            }


            return View(model);

        }


        //adds a new purchase detail for the current order number

        //[Bind(Include = "")] Purchase_Order_Record por, 
        [HttpPost]
        public ActionResult RaisePurchaseOrder([Bind(Include = "orderNo, itemCode, qty, remarks, price")]Purchase_Details pd)
        {

            int orderNo = findNextOrderNo();

            ViewBag.orderNo = orderNo;

            List<Purchase_Details> model = new List<Purchase_Details>();

            if (Session["purchaseOrder"] != null) {
                model = (List<Purchase_Details>)Session["purchaseOrder"];
            }
            
            model.Add(pd);
           

            Session["purchaseOrder"] = model;

            return View("RaisePurchaseOrder", model);

            
        }

        [HttpGet]
        public ActionResult GeneratePO()
        {

            int orderNo = findNextOrderNo();

            List<Purchase_Details> po = (List<Purchase_Details>) Session["purchaseOrder"];

            Purchase_Order_Record por = new Purchase_Order_Record();
            por.clerkID = "S1017"; // supposed to be currently logged in guy
            por.date = DateTime.Now;
            por.orderNo = orderNo;
            por.status = "incomplete"; //the default starting status
            por.supplierCode = "ALPA";
            por.expectedDeliveryDate = DateTime.Now.AddDays(14); //hard coded currently
            pos.AddNewPurchaseOrder(por);

            foreach (Purchase_Details pd in po)
            {
                pos.AddPurchaseDetail(pd);

            }

            Session["purchaseOrder"] = null;


            List<Purchase_Order_Record> model = pos.GetAllPurchaseOrder();

            return View("ListPurchaseOrders", model);
        }

        //delete purchase order
        [HttpGet]
        public ActionResult Delete(string id)
        {

            pos.DeletePurchaseOrder(Int32.Parse(id));
            return View("ListPurchaseOrders");

        }

        //delete purchase detail
        [HttpGet]
        public ActionResult DeletePD(string id)
        {
            List<Purchase_Details> model = (List<Purchase_Details>)Session["purchaseOrder"];
            Purchase_Details pd = model.Where(x => x.itemCode == id).First();
            model.Remove(pd);

            return View("RaisePurchaseOrder", model );

        }

        [HttpGet]
        public ActionResult ClearSession()
        {
            Session["purchaseOrder"] = null;
            List<Purchase_Details> model = (List<Purchase_Details>)Session["purchaseOrder"];

            return View("RaisePurchaseOrder", model);
        }

       //helper method
       public int findNextOrderNo()
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                int maxOrderNo = 0;
                //to obtain highest order number
                List<Purchase_Order_Record> pds = Entity.Purchase_Order_Record.ToList();
                foreach (Purchase_Order_Record p in pds)
                {
                    maxOrderNo = 1;
                    if (p.orderNo > maxOrderNo)
                    {
                        maxOrderNo = p.orderNo;
                    }

                }

                return maxOrderNo + 1;
            }

            

        }

       
    }
}
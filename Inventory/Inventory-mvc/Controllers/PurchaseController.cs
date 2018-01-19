using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: PurchaseDetails

        IPurchaseOrderService pos = new PurchaseOrderService();
        Purchase_Order_Record por = new Purchase_Order_Record();
        IStationeryService ss = new StationeryService();
        Dictionary<Purchase_Details, string> details = new Dictionary<Purchase_Details, string>();
        StationeryModel ctx = new StationeryModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //FormCollection form
        public ActionResult ListPurchaseOrders(string search, string searchBy)
        {

            List<Purchase_Order_Record> model = new List<Purchase_Order_Record>();
            List<Purchase_Order_Record> searchResults = new List<Purchase_Order_Record>();
            switch (searchBy)
            {
                case ("orderNo"):
                    por = pos.FindByOrderID(Int32.Parse(search));
                    model.Add(por);
                    break;

                case ("status"):
                    searchResults = pos.FindByStatus(search);
                    model.AddRange(searchResults);
                    break;

                case ("supplier"):
                    searchResults = pos.FindBySupplier(search);
                    model.AddRange(searchResults);
                    break;

                default:
                    model = pos.GetAllPurchaseOrder();
                    break;

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

            if (Session["detailsBundle"] != null)
            {
                //model = (List<Purchase_Details>)Session["purchaseOrder"];
                details = (Dictionary<Purchase_Details, string>)Session["detailsBundle"];
                model = details.Keys.ToList<Purchase_Details>();

            }


            return View(model);

        }


        [HttpPost]
        public ActionResult RaisePurchaseOrder([Bind(Include = "orderNo, itemCode, qty, remarks, price")]Purchase_Details pd, string supplierCode)
        {

            int orderNo = findNextOrderNo();
            ViewBag.orderNo = orderNo;
            List<Purchase_Details> model = new List<Purchase_Details>();
            Dictionary<Purchase_Details, string> details = new Dictionary<Purchase_Details, string>();

            if (Session["detailsBundle"] != null)
            {
                //model = (List<Purchase_Details>)Session["purchaseOrder"];
                details = (Dictionary<Purchase_Details, string>)Session["detailsBundle"];
                model = details.Keys.ToList<Purchase_Details>();

            }


            details.Add(pd, supplierCode);
            model.Add(pd);
            Session["detailsBundle"] = details;
            return View("RaisePurchaseOrder", model);


        }

        //gets the purchase details and supplier to order from - which are bundled together as key-value pairs, then creates a new purchase order for each supplier
        //then creates purchase detail with order num matching the supplier which user has chosen
        [HttpGet]
        public ActionResult GeneratePO()
        {

            int orderNo = findNextOrderNo();
            List<Purchase_Details> po = details.Keys.ToList<Purchase_Details>();
            details = (Dictionary<Purchase_Details, string>)Session["detailsBundle"];

            List<string> suppliers = details.Values.Distinct().ToList();
            
            for (int i = 0; i < suppliers.Count; i++)
            {
                Purchase_Order_Record p = new Purchase_Order_Record();
                p.clerkID = "S1017"; // HARD CODED supposed to be currently logged in guy
                p.date = DateTime.Now;
                p.orderNo = orderNo + i;
                p.status = "incomplete"; //the default starting status
                p.supplierCode = suppliers[i];
                p.expectedDeliveryDate = DateTime.Now.AddDays(14); //HARD CODED currently
                pos.AddNewPurchaseOrder(p);


                foreach (KeyValuePair<Purchase_Details, string> entry in details)
                {
                    Purchase_Details pd = entry.Key;

                    string supplierCode = entry.Value;
                    //find the orderNo matching supplier and create the purchase detail 
                    if (supplierCode == suppliers[i])
                    {
                        pd.orderNo = orderNo + i;
                        pos.AddPurchaseDetail(pd);
                    }

            }
            }

            Session["detailsBundle"] = null; //clear the orderCart
            List<Purchase_Order_Record> model = pos.GetAllPurchaseOrder();
            return View("ListPurchaseOrders", model);
        }

        //delete purchase order record
        [HttpGet]
        public ActionResult Delete(string id)
        {
        
            pos.DeletePurchaseOrder(Int32.Parse(id));
            return View("ListPurchaseOrders");

        }

        //delete purchase detail record
        [HttpGet]
        public ActionResult DeletePD(string id)
        {
            Dictionary<Purchase_Details, string> details = (Dictionary<Purchase_Details, string>)Session["detailsBundle"];
            List<Purchase_Details> model = details.Keys.ToList<Purchase_Details>();
            Purchase_Details pd = model.Where(x => x.itemCode == id).First();
            model.Remove(pd);

            return View("RaisePurchaseOrder", model);

        }

        [HttpGet]
        public ActionResult ClearSession()
        {
            Session["detailsBundle"] = null;

            List<Purchase_Details> model = new List<Purchase_Details>();

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

        ////helper method to refactor into service
        //public String ConvertSupplierNameToCode(string name)
        //{
        //    String s = ctx.Supplier.Where(x=>x.supplierName == name).First().supplierCode;

        //    return s;
        //}

    }

}

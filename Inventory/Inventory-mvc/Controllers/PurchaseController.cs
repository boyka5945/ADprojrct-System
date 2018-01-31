﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: PurchaseDetails

        IPurchaseOrderService pos = new PurchaseOrderService();
        Purchase_Order_Record por = new Purchase_Order_Record();
        IStationeryService ss = new StationeryService();
        Dictionary<Purchase_Detail, string> details = new Dictionary<Purchase_Detail, string>();
        StationeryModel ctx = new StationeryModel();
        ISupplierService supplierService = new SupplierService();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //FormCollection form
        public ActionResult ListPurchaseOrders(string search, string searchBy, int? page)
        {

            ViewBag.Search = search;
            ViewBag.SearchBy = searchBy;
            List<Purchase_Order_Record> model = new List<Purchase_Order_Record>();
            List<Purchase_Order_Record> searchResults = new List<Purchase_Order_Record>();

            try
            {
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
            }
            catch
            {
                model = new List<Purchase_Order_Record>();
                //return empty list if nothing can be found

            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
            //return View(model);
        }

        //[HttpGet]
        //public ActionResult ObtainPurchaseDetails(string id)
        //{
        //    return View();

        //}

        [HttpGet]
        public ActionResult RaisePurchaseOrder()
        {
            //to obtain latest order number
            int orderNo = findNextOrderNo();

            ViewBag.orderNo = orderNo;

            List<Purchase_Detail> model = new List<Purchase_Detail>();
            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.AllItems = ss.GetAllStationery();
            ViewBag.supplierList = supplierService.GetAllSuppliers();

            if (Session["detailsBundle"] != null)
            {
                //model = (List<Purchase_Detail>)Session["purchaseOrder"];
                details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
                model = details.Keys.ToList<Purchase_Detail>();

            }


            return View(model);

        }


        [HttpPost]
        public ActionResult RaisePurchaseOrder([Bind(Include = "orderNo, itemCode, qty, remarks, price")]Purchase_Detail pd, string supplierCode)
        {

            int orderNo = findNextOrderNo();
            ViewBag.orderNo = orderNo;

            List<Purchase_Detail> model = new List<Purchase_Detail>();
            Dictionary<Purchase_Detail, string> details = new Dictionary<Purchase_Detail, string>();

            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.supplierList = supplierService.GetAllSuppliers();


            if (Session["detailsBundle"] != null)
            {

                details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
                model = details.Keys.ToList<Purchase_Detail>();

            }

            if (pd.itemCode != "--Select--")
            {
                Session["pd"] = pd;
                Session["description"] = ss.FindStationeryByItemCode(pd.itemCode).description;
                var a = ss.FindStationeryByItemCode(pd.itemCode).price;
                    Session["price"] = ss.FindStationeryByItemCode(pd.itemCode).price;
                //Session["SupplierCode"] = supplierCode;
                //return View(model);
            }

            if (supplierCode != "--Select--")
            {
                Session["SupplierCode"] = supplierCode;
                Session["sup"] = supplierService.FindBySupplierCode(supplierCode).SupplierName;
                
            }

            //validation
            if (pd.qty < 1)
            {
                TempData["QtyErrorMessage"] = "Quantity should be greater than 0";
                string qtyError = "Quantity should be greater than 0";
                ModelState.AddModelError("qty", qtyError);

            }
            if (pd.price <= 0)
            {
                TempData["PriceErrorMessage"] = "Price cannot be 0 or less than 0.";
                string priceError = "Price cannot be 0 or less than 0. ";
                ModelState.AddModelError("price", priceError);
            }

            if (supplierCode == "--Select--")
            {
                TempData["SupplierCodeErrorMessage"] = "SupplierCode must be selected.";
                string supplierCodeError = "SupplierCode must be selected. ";
                ModelState.AddModelError("supplierCode", supplierCodeError);
            }

            if (pd.itemCode == "--Select--")
            {
                TempData["ItemCodeErrorMessage"] = "ItemCode must be selected.";
                string itemCodeError = "ItemCode must be selected. ";
                ModelState.AddModelError("itemCode", itemCodeError);
                return View(model);
            }

            if (pd.qty == 0)
            {
                return View(model);
            }

            if (pd.price <= 0)
            {
                return View(model);
            }




            //only save to session if itemcode, price and qty are entered
            if (pd.price > 0 && pd.qty > 0 && pd.itemCode != ("--Select--") && supplierCode != ("--Select--"))
            {

                //check for pre-exisiting pd with same itemcode
                if (model.Exists(x => x.itemCode == pd.itemCode))
                {
                    TempData["DuplicateEntryMessage"] = "An order for the same item already exists in the list.";

                }
                //save
                else
                {
                    //pd.price = ctx.Stationeries.Where(x => x.itemCode == pd.itemCode).First().price;
                    details.Add(pd, supplierCode);
                    model.Add(pd);
                    if (Session["pd"] != null)
                    {
                        Session["pd"] = null;
                        Session["description"] = null;
                        Session["price"] = null;
                        Session["SupplierCode"] = null;
                    }
                    Session["detailsBundle"] = details;
                }
                ModelState.Clear();
                return View("RaisePurchaseOrder", model);

            }

            ModelState.Clear();
            return View(model);
        }

        //gets the purchase details and supplier to order from - which are bundled together as key-value pairs, then creates a new purchase order for each supplier
        //then creates purchase detail with order num matching the supplier which user has chosen
        [HttpGet]
        public ActionResult GeneratePO(int? page)
        {
            string loggedInUser = HttpContext.User.Identity.Name;
            

            int orderNo = findNextOrderNo();
            List<Purchase_Detail> po = details.Keys.ToList<Purchase_Detail>();
            details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
            if (details == null)
            {
                TempData["ErrorMessage"] = "No PO to generate.";
                return RedirectToAction("RaisePurchaseOrder");
            }
            else
            {
                List<string> suppliers = details.Values.Distinct().ToList();

                for (int i = 0; i < suppliers.Count; i++)
                {              
                    Purchase_Order_Record p = new Purchase_Order_Record();
                    p.clerkID = loggedInUser; 
                    p.date = DateTime.Now;
                    p.orderNo = orderNo + i;
                    p.status = "incomplete"; //the default starting status
                    p.supplierCode = suppliers[i];
                    p.expectedDeliveryDate = DateTime.Now.AddDays(14); //HARD CODED currently
                    pos.AddNewPurchaseOrder(p);


                    foreach (KeyValuePair<Purchase_Detail, string> entry in details)
                    {
                        Purchase_Detail pd = entry.Key;

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
                //return View("ListPurchaseOrders", model);
                int pageSize = 6;
                int pageNumber = (page ?? 1);
                return View("ListPurchaseOrders", model.ToPagedList(pageNumber, pageSize));
            }

        }

        //delete purchase order record
        [HttpGet]
        public ActionResult Delete(string id, int? page)
        {

            pos.DeletePurchaseOrder(Int32.Parse(id));
            List<Purchase_Order_Record> model = pos.GetAllPurchaseOrder();

            //return View("ListPurchaseOrders");
            int pageSize = 6;
            int pageNumber = (page ?? 1);


            return View("ListPurchaseOrders", model.ToPagedList(pageNumber, pageSize));


        }

        //delete purchase detail record
        [HttpGet]
        public ActionResult DeletePD(string id)
        {
            Dictionary<Purchase_Detail, string> details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
            List<Purchase_Detail> model = details.Keys.ToList<Purchase_Detail>();
            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.supplierList = supplierService.GetAllSuppliers();



            Purchase_Detail pd = model.Where(x => x.itemCode == id).First();

            //remove from model
            model.Remove(pd);

            //remove from sessions state
            details.Remove(pd);
            Session["detailsBundle"] = details;





            return View("RaisePurchaseOrder", model);

        }

        [HttpGet]
        public ActionResult ClearSession()
        {
            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.supplierList = supplierService.GetAllSuppliers();

            Session["detailsBundle"] = null;

            List<Purchase_Detail> model = new List<Purchase_Detail>();

            return View("RaisePurchaseOrder", model);
        }

        [HttpPost]
        public ActionResult UpdatePD()
        {
            int qty = Int32.Parse(Request.Params.Get("dqty"));
            decimal price = Decimal.Parse(Request.Params.Get("dprice"));

            Dictionary<Purchase_Detail, string> details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
            List<Purchase_Detail> model = details.Keys.ToList<Purchase_Detail>();
            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.supplierList = supplierService.GetAllSuppliers();

            string itemCode = Request.Params.Get("ditemCode");
            var index = model.FindIndex(c => c.itemCode == itemCode);

            
            qty = Int32.Parse(Request.Params.Get("dqty"));
            if (qty < 1)
            {
                TempData["dQtyErrorMessage"] = "Quantity should be greater than 0";
                //return View("UpdatePD", "Purchase", FormMethod.Post);

            }
            else
            {
                model[index].qty = qty;
            }

             price = Int32.Parse(Request.Params.Get("dprice"));
            if (price <= 0)
            {
                TempData["dPriceErrorMessage"] = "Price cannot be 0 or less than 0.";


                //return View("UpdatePD", "Purchase", FormMethod.Post);

            }
            else
            {
                model[index].price = price;
            }


            string supplier = details[model[index]];

            //save changes to the dictionary and to session state
            details.Remove(model[index]);
            details.Add(model[index], supplier);

            Session["detailsBundle"] = details;


            return View("RaisePurchaseOrder", model);

        }

        //for refreshing the values of price and description whenever drop down list value is changed
        [HttpGet]
        public ActionResult GetDescrpAndPrice(string itemcode)
        {
            Dictionary<Purchase_Detail, string> details = new Dictionary<Purchase_Detail, string>();
            List<Purchase_Detail> model = new List<Purchase_Detail>();
            if (Session["detailsBundle"] != null)
            {
                details = (Dictionary<Purchase_Detail, string>)Session["detailsBundle"];
                model = details.Keys.ToList<Purchase_Detail>();
            }
            ViewBag.itemCodeList = ss.GetAllItemCodes();
            ViewBag.supplierList = supplierService.GetAllSuppliers();

            Stationery s = ss.FindStationeryByItemCode(itemcode);
            ViewBag.price = s.price;
            ViewBag.descrp = s.description;
            ViewBag.itemcode = itemcode;


            return View("RaisePurchaseOrder", model);

        }

        //helper method
        public int findNextOrderNo()
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                int maxOrderNo = 0;
                //to obtain highest order number
                List<Purchase_Order_Record> pds = Entity.Purchase_Order_Records.ToList();
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

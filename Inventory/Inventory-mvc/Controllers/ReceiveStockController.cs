﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.Service;



namespace Inventory_mvc.Controllers
{
    public class ReceiveStockController : Controller
    {
        IReceiveStockService receiveStockService = new ReceiveStockService();
        // GET: ReceiveStock

        StationeryModel ctx = new StationeryModel();
        PurchaseOrderService pos = new PurchaseOrderService();
        IStationeryService ss = new StationeryService();
        ITransactionRecordService trs = new TransactionRecordService();



        public ActionResult Index()
        {
            return View();

        }
        //Clerk
        [HttpGet]
        public ActionResult StockReceive()
        {
            //empty model
            List<Purchase_Detail> model = new List<Purchase_Detail>();

            if (TempData["POnumRedirect"] != null)
            {
                int poNum = Int32.Parse((string) TempData["POnumRedirect"]);
                model = pos.GetPurchaseDetailsByOrderNo(poNum);

                ViewBag.DOnumber = TempData["DOnumRedirect"];
                ViewBag.POnumber = TempData["PONumRedirect"];
                ViewBag.Supplier = TempData["SupplierRedirect"];
                ViewBag.ReceivedDate = TempData["ReceivedDateRedirect"];

            }
            return View(model);



        }
        //CLERK
        [HttpPost]
        public ActionResult StockReceive(string searchPONumber)
        {
            List<Purchase_Detail> model = new List<Purchase_Detail>();
            try
            {
                model = pos.GetPurchaseDetailsByOrderNo(Int32.Parse(searchPONumber));
                Purchase_Order_Record por = pos.FindByOrderID(Int32.Parse(searchPONumber));

                ViewBag.PONumber = searchPONumber;
                ViewBag.Supplier = ctx.Suppliers.Where(x => x.supplierCode == por.supplierCode).First().supplierName;
            }

            catch
            {
                TempData["searchError"] = "PO Number is required";
                //return RedirectToAction("StockReceive");
            }
            return View(model);
        }

        //UpdateReceived action must write to the following tables; 

        //update qty in stationery table
        //update POR status
        //update PD fulfilled qty, remarks,deliveryNo
        //update transaction table


        //CLERK
        [HttpGet]
        public ActionResult UpdateReceived(string DONumber, string ReceivedDate, string PONumber, string supplier, string sbutton)
        {

            string clerkID = HttpContext.User.Identity.Name;

            Purchase_Order_Record por = pos.FindByOrderID(Int32.Parse(PONumber));
            int pdOutstanding = por.Purchase_Detail.Count;



            //update transaction records table
            Transaction_Record tr = new Transaction_Record();
            int nextTransactionNo = findNextTransactionNo();
            tr.transactionNo = nextTransactionNo;
            tr.clerkID = clerkID;
            tr.date = DateTime.Now;
            tr.type = "DO-" + DONumber;
            trs.AddNewTransactionRecord(tr);

            List<Purchase_Detail> model = pos.GetPurchaseDetailsByOrderNo(Int32.Parse(PONumber));
            List<string> filledItems = new List<string>();
            List<string> unfilledItems = new List<string>();

            ////validation - fulfilled items cannot be filled beyond requested limit
            //foreach (Purchase_Detail pd in model)
            //{
            //    if (pd.fulfilledQty == pd.qty)
            //    {
            //        filledItems.Add(pd.Stationery.description);

            //    }

            //}
            ////confirm that the form is invalid
            //if (filledItems.Count > 0)
            //{
            //    TempData["icodesFilled"] = filledItems;
            //    //return the existing form values back to the view
            //    TempData["DOnumRedirect"] = DONumber;
            //    TempData["PONumRedirect"] = PONumber;
            //    TempData["SupplierRedirect"] = supplier;
            //    TempData["ReceivedDateRedirect"] = ReceivedDate;

            //    return RedirectToAction("StockReceive");

            //}

            //total number of rows updated
            int totalItemsUpdated = 0;

            foreach (Purchase_Detail pd in model)
            {

                string ic = pd.itemCode;
                string received = Request.QueryString.GetValues("num-" + ic).First();
                int receivedNum = Int32.Parse(received);


                //condition check if option for button" all received"
                if (sbutton == "Submit All")
                {

                    if (pd.fulfilledQty == null)
                    {
                        pd.fulfilledQty = 0;
                    }
                    receivedNum = pd.qty - pd.fulfilledQty.Value;

                }

                if (receivedNum != 0)
                {


                    //else
                    //{
                    //    receivedNum = Int32.Parse(received);
                    //}
                    string remarks = Request.QueryString.GetValues("rem-" + ic).First();
                    pd.fulfilledQty += receivedNum;

                    pd.remarks = remarks; //delete?

                    //update stationery table
                    Stationery s = (from x in ctx.Stationeries
                                    where x.itemCode == ic
                                    select x).First();
                    s.stockQty += receivedNum;

                    ctx.SaveChanges();
                    totalItemsUpdated++; //update number of rows updated

                    //update transaction details table
                    Transaction_Detail td = new Transaction_Detail();
                    td.itemCode = pd.itemCode;
                    td.adjustedQty = receivedNum;
                    td.balanceQty = s.stockQty + receivedNum;
                    td.remarks = remarks; //delete?
                    td.transactionNo = nextTransactionNo;
                    AddNewTransactionDetail(td);

                    //update purchase details table
                    pos.UpdatePurchaseDetailsInfo(pd);




                    //if (pd.deliveryOrderNo.HasValue) //append to existing DO numbers
                    //{
                    //    pd.deliveryOrderNo = pd.deliveryOrderNo 

                    //}
                    //else 
                    //{
                    //    pd.deliveryOrderNo = Int32.Parse(DONumber);

                    //}

                    pd.deliveryOrderNo = Int32.Parse(DONumber); //need to update this code once DB is altered

                    //need to validate, to ensure that qty received is not higher than qty ordered



                    //update purchase order record to partially fulfilled
                    por.status = "partially fulfilled";
                    pos.UpdatePurchaseOrderInfo(por);



                    if (pd.qty == pd.fulfilledQty)
                    {
                        pdOutstanding--;

                    }
                }
            }

            if(totalItemsUpdated == 0)
            {
                TempData["Items updated"] = "No records saved";
            }

            //check if all pd in por are fulfilled, and update por table
            if (pdOutstanding == 0)
            {
                por.status = "completed";
                pos.UpdatePurchaseOrderInfo(por);
            }


            ViewBag.Supplier = supplier;
            ViewBag.PONumber = PONumber;
            return View("StockReceive", model);

        }


        //CLERK
        //helper methods
        public int findNextTransactionNo()
        {
            //find latest transaction no.

            List<Transaction_Record> trList = ctx.Transaction_Records.ToList();
            List<int> trno = new List<int>();
            if (trList.Count > 0)
            {
                foreach (Transaction_Record tr in trList)
                {
                    trno.Add(tr.transactionNo);

                }

                int nextNo = trno.Max() + 1;

                return nextNo;
            }
            else
            {
                return 1;
            }
        }
        //CLERK
        //transfer this code to transactionDAO
        public bool AddNewTransactionDetail(Transaction_Detail td)
        {
            using (StationeryModel ctx = new StationeryModel())
            {
                ctx.Transaction_Details.Add(td);
                ctx.SaveChanges();
                return true;
            }
        }
        //CLERK
        //transfer to transactionDAO

        public bool AddNewTransactionRecord(Transaction_Record tr)
        {
            using (StationeryModel ctx = new StationeryModel())
            {
                ctx.Transaction_Records.Add(tr);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
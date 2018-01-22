using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.Service;

//write to 3 tables upon updating quantity

namespace Inventory_mvc.Controllers
{
    public class ReceiveStockController : Controller
    {
        IReceiveStockService receiveStockService = new ReceiveStockService();
        // GET: ReceiveStock

        StationeryModel ctx = new StationeryModel();
        PurchaseOrderService pos = new PurchaseOrderService();
        IStationeryService ss = new StationeryService();


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StockReceive()
        {
            //empty model
            List<Purchase_Detail> model = new List<Purchase_Detail>();

            return View(model);



        }

        [HttpPost]
        public ActionResult StockReceive(string searchPONumber)
        {
            List<Purchase_Detail> model = pos.GetPurchaseDetailsByOrderNo(Int32.Parse(searchPONumber));
            Purchase_Order_Record por = pos.FindByOrderID(Int32.Parse(searchPONumber));

            ViewBag.PONumber = searchPONumber;
            ViewBag.Supplier = ctx.Suppliers.Where(x => x.supplierCode == por.supplierCode).First().supplierName;

            return View(model);
        }

        //must write to three tables
        [HttpGet]
        public ActionResult UpdateReceived(string DONumber, string ReceivedDate, string PONumber)
        {
            string clerkID = "Alex"; //HARD CODED
            Purchase_Order_Record por = pos.FindByOrderID(Int32.Parse(PONumber));
            int pdOutstanding = por.Purchase_Detail.Count;


            List<Purchase_Detail> model = pos.GetPurchaseDetailsByOrderNo(Int32.Parse(PONumber));
            foreach (Purchase_Detail pd in model)
            {
                string ic = pd.itemCode;
                string received = Request.QueryString.GetValues("num-" + ic).First();
                int receivedNum = Int32.Parse(received);
                string remarks = Request.QueryString.GetValues("rem-" + ic).First();
                pd.fulfilledQty = receivedNum;
                pd.remarks = remarks;
                pd.deliveryOrderNo = Int32.Parse(DONumber);
                //need to validate, to ensure that qty received is not higher than qty ordered

               //update purchase details table
                pos.UpdatePurchaseDetailsInfo(pd);

                //update stationery table
                Stationery s = (from x in ctx.Stationeries
                                where x.itemCode == ic
                                select x).First();
                s.stockQty += receivedNum;

                ctx.SaveChanges();

                //
               


                

                if (pd.fulfilledQty < pd.qty)
                {
                    pdOutstanding--;

                }

            }
            //check if all pd in por are fulfilled, and update por table
            if (pdOutstanding ==0)
            {
                por.status = "completed";
                pos.UpdatePurchaseOrderInfo(por);
            }

            else if (pdOutstanding< por.Purchase_Detail.Count)
            {

            }

            //update transaction table

         



            return View("StockReceive", model);

        }


    }
}
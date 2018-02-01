using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Entity;
using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.Function;
using Rotativa.MVC;

namespace Inventory_mvc.Controllers
{
    public class POGeneratorController : Controller
    {
        Inventory_mvc.Models.StationeryModel ctx = new Models.StationeryModel();
        PurchaseOrderService pos = new PurchaseOrderService();

        // GET: POGenerator
        //CLERK
        [RoleAuthorize]
        [HttpGet]
        public ActionResult gen(string id) //id is purchase order number
        {

            int orderNo = Int32.Parse(id);
            Purchase_Order_Record por = pos.FindByOrderID(orderNo);

            Inventory_mvc.Models.User clerk = ctx.Users.Where(x => x.userID == por.clerkID).First();

            Supplier s = ctx.Suppliers.Where(x => x.supplierCode == por.supplierCode).First();
            ViewBag.orderNo = id;
            ViewBag.clerkID = por.clerkID;
            ViewBag.clerkName = clerk.name;
            ViewBag.supplier = s.supplierName;
            ViewBag.delivery = por.expectedDeliveryDate;

            List<Purchase_Detail> model = ctx.Purchase_Detail.Where(x => x.orderNo == orderNo).ToList();

            return View(model);
        }

        //CLERK
        [RoleAuthorize]
        [HttpGet]
        public ActionResult GeneratePDF(string id) //id is purchase order number
        {
            int orderNo = Int32.Parse(id);
            Purchase_Order_Record por = pos.FindByOrderID(orderNo);

            Inventory_mvc.Models.User clerk = ctx.Users.Where(x => x.userID == por.clerkID).First();

            Supplier s = ctx.Suppliers.Where(x => x.supplierCode == por.supplierCode).First();
            ViewBag.orderNo = id;
            ViewBag.clerkID = por.clerkID;
            ViewBag.clerkName = clerk.name;
            ViewBag.supplier = s.supplierName;
            ViewBag.delivery = por.expectedDeliveryDate;

            List<Purchase_Detail> model = ctx.Purchase_Detail.Where(x => x.orderNo == orderNo).ToList();

            string fileName = String.Format("{0}_PO_{1}.pdf", s.supplierCode, id);
            return new ViewAsPdf("_GeneratePDF", model) { FileName = fileName };


        }
    }
}
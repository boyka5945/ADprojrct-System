using System;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockReceive(string searchString, int? page)
        {
            


            return View();
            
            
           
        }
    }
}
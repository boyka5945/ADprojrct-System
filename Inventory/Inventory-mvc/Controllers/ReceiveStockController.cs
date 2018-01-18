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
        // GET: ReceiveStock
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockReceive(string searchString, int? page)
        {
            List<Purchase_Detail> purchase_detail = stationeryService.GetAllStationery();
            

            if (categoryID == "All")
            {
                ViewBag.CategoryID = "All";
            }
            else
            {
                ViewBag.CategoryID = categoryID;
                stationeries = (from s in stationeries
                                where s.categoryID.ToString() == categoryID
                                select s).ToList();
            }

            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                string[] searchStringArray = searchString.Split();
                foreach (string s in searchStringArray)
                {
                    string search = s.ToLower().Trim();
                    if (!String.IsNullOrEmpty(search))
                    {
                        stationeries = (from x in stationeries
                                        where x.description.ToLower().Contains(search)
                                        select x).ToList();
                    }
                }
            }

            ViewBag.Page = page;

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }
    }
}
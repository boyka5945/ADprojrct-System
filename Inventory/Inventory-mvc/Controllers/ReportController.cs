using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Models;


namespace Inventory_mvc.Controllers
{
    public class ReportController : Controller
    {
        IReportService reportService = new ReportService();
        IStationeryService stationeryService = new StationeryService();

        // GET: Report
        public ActionResult Report()
        {

            List<ReportViewModel> vmList = reportService.RetrieveQty();
            List<ReportViewModel> result = new List<ReportViewModel>();
            foreach (var vm in vmList)
            {
                ReportViewModel i = new ReportViewModel();
                i.CategoryName = vm.CategoryName;
                i.Qty = vm.Qty;

                bool contain = false;

                foreach (var j in result)
                {
                    if (i.CategoryName == j.CategoryName)
                    {
                        j.Qty += i.Qty;
                        contain = true;
                        break;
                    }
                }

                if (!contain)
                {
                    result.Add(i);
                }
            }

            List<Category> categoryList = stationeryService.GetAllCategory();
            List<string> categoryArray = new List<string>();
            List<int> quantity = new List<int>();

            
            foreach(var c in categoryList)
            {
                categoryArray.Add(c.categoryName);
            }

            for (int i = 0; i < categoryArray.Count; i++)
            {
                quantity.Add(0);
            }

            foreach (var item in result)
            {
               for(int i = 0; i < categoryArray.Count; i++)
                {
                    if(categoryArray[i] == item.CategoryName)
                    {
                        quantity[i] += item.Qty;
                    }
                }
            }

            var cat = categoryArray.ToArray();
            var qty = quantity.ToArray();

            ViewBag.X = cat;
            ViewBag.Y = qty;

            TempData["X"] = cat;
            TempData["Y"] = qty;

            return View();
        }

      
    }
}
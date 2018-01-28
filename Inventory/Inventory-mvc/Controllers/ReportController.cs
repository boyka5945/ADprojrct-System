using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Models;
using Newtonsoft.Json;
using System.Data;

namespace Inventory_mvc.Controllers
{
    public class ReportController : Controller
    {
        IReportService reportService = new ReportService();
        IStationeryService stationeryService = new StationeryService();

        private string[] months = new string[]{ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        // GET: Report
        public ActionResult Report()
        {
            //DateTime todayDate = DateTime.Now.Date;
            //List<ReportViewModel> vmList = reportService.RetrieveQty(todayDate,todayDate);
            //List<ReportViewModel> result = new List<ReportViewModel>();
            //foreach (var vm in vmList)
            //{
            //    ReportViewModel i = new ReportViewModel();
            //    i.CategoryName = vm.CategoryName;
            //    i.RequestQuantity = vm.RequestQuantity;

            //    bool contain = false;

            //    foreach (var j in result)
            //    {
            //        if (i.CategoryName == j.CategoryName)
            //        {
            //            j.RequestQuantity += i.RequestQuantity;
            //            contain = true;
            //            break;
            //        }
            //    }

            //    if (!contain)
            //    {
            //        result.Add(i);
            //    }
            //}

            //List<Category> categoryList = stationeryService.GetAllCategory();
            //List<string> categoryArray = new List<string>();
            //List<int> quantity = new List<int>();



            //foreach (var c in categoryList)
            //{
            //    categoryArray.Add(c.categoryName);
            //}

            //for (int i = 0; i < categoryArray.Count; i++)
            //{
            //    quantity.Add(0);
            //}

            //foreach (var item in result)
            //{
            //   for(int i = 0; i < categoryArray.Count; i++)
            //    {
            //        if(categoryArray[i] == item.CategoryName)
            //        {
            //            quantity[i] += item.RequestQuantity;
            //        }
            //    }
            //}

           

            //var cat = categoryArray.ToArray();
            //var qty = quantity.ToArray();

          
            //ViewBag.X = cat;
            //ViewBag.Y = qty;
           

            return View();
        }

        [HttpPost]
        public ActionResult Report(DateTime from, DateTime toto)
        {

            //List<ReportViewModel> vmList = reportService.RetrieveQty(from,toto);
            //List<ReportViewModel> result = new List<ReportViewModel>();
            //foreach (var vm in vmList)
            //{
            //    ReportViewModel i = new ReportViewModel();
            //    i.CategoryName = vm.CategoryName;
            //    i.RequestQuantity = vm.RequestQuantity;

            //    bool contain = false;

            //    foreach (var j in result)
            //    {
            //        if (i.CategoryName == j.CategoryName)
            //        {
            //            j.RequestQuantity += i.RequestQuantity;
            //            contain = true;
            //            break;
            //        }
            //    }

            //    if (!contain)
            //    {
            //        result.Add(i);
            //    }
            //}

            //List<Category> categoryList = stationeryService.GetAllCategory();
            //List<string> categoryArray = new List<string>();
            //List<int> quantity = new List<int>();



            //foreach (var c in categoryList)
            //{
            //    categoryArray.Add(c.categoryName);
            //}

            //for (int i = 0; i < categoryArray.Count; i++)
            //{
            //    quantity.Add(0);
            //}

            //foreach (var item in result)
            //{
            //    for (int i = 0; i < categoryArray.Count; i++)
            //    {
            //        if (categoryArray[i] == item.CategoryName)
            //        {
            //            quantity[i] += item.RequestQuantity;
            //        }
            //    }
            //}



            //var cat = categoryArray.ToArray();
            //var qty = quantity.ToArray();


            //ViewBag.X = cat;
            //ViewBag.Y = qty;


            return View();
        }



        [HttpGet]
        public ActionResult ItemRequestTrend()
        {
            ViewBag.ItemCode = "";
            return View();
        }


        [HttpPost]
        public ActionResult ItemRequestTrend(string itemCode, int[] years)
        {
           
            if(itemCode != null & years.Length != 0 )
            {
                years = years.OrderBy(x => x.ToString()).ToArray();
                List<ReportViewModel> vmList = reportService.GetItemRequestTrend(itemCode, years);
               
                var results = (from vm in vmList
                               group vm by new { vm.Year, vm.Month } into g
                               select new { Year = g.Key.Year, Month = g.Key.Month, Sum = g.Sum(v => v.RequestQuantity) });

                Dictionary<string, int[]> dataset = new Dictionary<string, int[]>();

                foreach(var y in years)
                {
                    string key = y.ToString(); // use year for dict key
                    int[] value = new int[12]; // 12 months
                    dataset.Add(key, value);                 
                }

                foreach(var r in results)
                {
                    string key = r.Year.ToString();
                    var value = dataset[key]; // get values based on year
                    value[r.Month - 1] = r.Sum; // put sum into corresponding month
                }

                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);

                ViewBag.ItemCode = String.Format("{0} ({1})", itemCode, s.description);
                ViewBag.XLabels = months;

                for(int i = 0; i < years.Length; i++) // create data array based on # of years
                {
                    string labelName = String.Format("Label{0}", i+1);
                    string dataName = String.Format("Data{0}", i+1);

                    ViewData[labelName] = dataset.Keys.ToArray()[i];
                    ViewData[dataName] = dataset.Values.ToArray()[i];
                }
            }
            
            return PartialView("_ItemRequestTrend");
        }


        public ActionResult GetStationeryListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(term);
            foreach (var s in stationeries)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.itemCode, s.description);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSelectableYearsJSON()
        {
            int baseYear = 2017;

            List<int> years = reportService.GetSelectableYears(baseYear);

            List<JSONForCombobox> options = new List<JSONForCombobox>();
            foreach(var y in years)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = y.ToString();
                option.text = y.ToString();
                options.Add(option);
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }



        // TODO - REMOVE THIS METHOD 
        public ActionResult GenerateRandomDataForRequisitionRecords()
        {
            reportService.GenerateRandomDataForRequisitionRecords();
            return RedirectToAction("Login", "Home");
        }
    }
}

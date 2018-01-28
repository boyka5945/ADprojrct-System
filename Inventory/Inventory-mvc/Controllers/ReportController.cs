using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Service;
using Inventory_mvc.Function;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Controllers
{
    public class ReportController : Controller
    {
        IReportService reportService = new ReportService();
        IStationeryService stationeryService = new StationeryService();

        private string[] months = new string[]{ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };


        public ActionResult Index()
        {
            return View();
        }


        // GET: Report
        public ActionResult Report()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult Report(DateTime from, DateTime toto)
        {
            return View();
        }

        [HttpGet]
        public ActionResult RequisitionCumulativeChart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRequisitionCumulativeBar(int year)
        {
            ViewBag.Year = year.ToString();

            List<ReportViewModel> vmList = reportService.GetApprovedRequisitionsOfYear(year);

            var results = (from vm in vmList
                           group vm by new { vm.Year, vm.Month } into g
                           select new { Year = g.Key.Year, Month = g.Key.Month, Sum = g.Sum(v => v.RequestQuantity) });

            ViewBag.XLabels = months;

            // Get data for bar chart
            ViewBag.YBarLabel = "Monthly";
            int[] monthlyQuantity = new int[12];
            foreach (var r in results)
            {
                monthlyQuantity[r.Month - 1] = r.Sum;
            }
            ViewBag.YBarData = monthlyQuantity;

            // Get data for line
            ViewBag.YLineLabel = "Cumulative";
            int[] cumulative = new int[12];
            int cumulativeQuantity = 0;
            for (int i = 0; i < monthlyQuantity.Length; i++)
            {
                cumulativeQuantity += monthlyQuantity[i];
                cumulative[i] = cumulativeQuantity;
            }
            ViewBag.YLineData = cumulative;

            return PartialView("_RequisitionCumulativeBar");
        }




        [HttpGet]
        public ActionResult ItemRequestTrend()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ItemRequestTrend(string categoryID, string itemCode, int[] years)
        {
            years = years.OrderBy(x => x.ToString()).ToArray();
            List<ReportViewModel> vmList = reportService.GetItemRequestTrend(categoryID, itemCode, years);
               
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

            //Stationery s = stationeryService.FindStationeryByItemCode(itemCode);

            //ViewBag.ItemCode = String.Format("{0} ({1})", itemCode, s.description);
            ViewBag.ItemCode = itemCode;

            ViewBag.XLabels = months;

            for(int i = 0; i < years.Length; i++) // create data array based on # of years
            {
                string labelName = String.Format("Label{0}", i+1);
                string dataName = String.Format("Data{0}", i+1);

                ViewData[labelName] = dataset.Keys.ToArray()[i];
                ViewData[dataName] = dataset.Values.ToArray()[i];
            }
            
            return PartialView("_ItemRequestTrend");
        }

        [HttpGet]
        public ActionResult MonthlyReorderAmount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDeptMonthlyReorderAmount(int year, int month = -1)
        {
            ViewBag.Year = year.ToString();
            ViewBag.Month = (month == -1) ? "" : months[month - 1]; // get string representation of months

            List<ReportViewModel> vmList;

            if(month == -1) // -1 for all months
            {
                vmList = reportService.GetApprovedRequisitionsOfYear(year);
            }
            else
            {
                vmList = reportService.GetApprovedRequisitionDetialsBasedOnYearAndMonth(year, month);
            }

            var results = (from vm in vmList
                           group vm by vm.RequesterDepartment into g
                           select new { Dept = g.Key, Cost = g.Sum(v => v.RequestQuantity * v.Cost) });

            List<string> dept = new List<string>();
            List<decimal> cost = new List<decimal>();

            foreach(var r in results)
            {
                dept.Add(r.Dept);
            }

            dept.Sort(); // fix the position of department showing on chart

            foreach(string d in dept)
            {
                foreach(var r in results)
                {
                    if(r.Dept == d)
                    {
                        cost.Add(Math.Round(r.Cost, 2));
                        break;
                    }
                }
            }
            

            ViewBag.XLabels = dept.ToArray();
            ViewBag.YBarData = cost.ToArray();

            return PartialView("_DeptMonthlyReorder");
        }


        public JsonResult GetCategoryListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Category> categories = stationeryService.GetAllCategory();

            if(!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                categories = (from c in categories
                              where c.categoryName.ToLower().Contains(term)
                              select c).ToList();
            }
            else
            {
                options.Add(new JSONForCombobox("-1", "All"));
            }

            foreach (var c in categories)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = c.categoryID.ToString();
                option.text = c.categoryName;
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStationeryListBasedOnCategoryJSON(string categoryID, string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(null, categoryID);

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                stationeries = (from s in stationeries
                                where s.description.ToLower().Contains(term) || s.itemCode.ToLower().Contains(term)
                                select s).ToList();
            }
     
            if (categoryID != "-1" && String.IsNullOrEmpty(term)) 
            {
                // only allow All if a specific category is chosen
                options.Add(new JSONForCombobox("-1", "All"));
            }

            foreach (var s in stationeries)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.itemCode, s.description);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSelectableYearsJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            int baseYear = reportService.GetEarliestYear();
            List<int> years = reportService.GetSelectableYears(baseYear);

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                years = (from y in years
                         where y.ToString().ToLower().Contains(term)
                         select y).ToList();
            }

            foreach (var y in years)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = y.ToString();
                option.text = y.ToString();
                options.Add(option);
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetSelectableMonthsJSON(int year, string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<int> selectableMonths = reportService.GetSelectableMonths(year);

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                selectableMonths = (from m in selectableMonths
                                    where months[m - 1].ToString().ToLower().Contains(term)
                                    select m).ToList();
            }
            else
            {
                options.Add(new JSONForCombobox("-1", "All"));
            }


            foreach (var m in selectableMonths)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = m.ToString();
                option.text = months[m - 1];
                options.Add(option);
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }




        // TODO - REMOVE THIS METHOD 
        //public ActionResult GenerateRandomDataForRequisitionRecords()
        //{
        //    reportService.GenerateRandomDataForRequisitionRecords();
        //    return RedirectToAction("Login", "Home");
        //}
    }
}

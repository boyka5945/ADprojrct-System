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
        //public ActionResult GenerateRandomDataForRequisitionRecords()
        //{
        //    reportService.GenerateRandomDataForRequisitionRecords();
        //    return RedirectToAction("Login", "Home");
        //}
    }
}

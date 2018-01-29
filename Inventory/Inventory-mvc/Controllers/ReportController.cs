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
        IDepartmentService departmentService = new DepartmentService();
        ISupplierService supplierService = new SupplierService();

        private string[] months = new string[]{ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult RequisitionCumulativeChart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRequisitionCumulativeBar(int year, string deptCode)
        {
            ViewBag.Year = year.ToString();

            if(deptCode == "-1")
            {
                ViewBag.Dept = "All Departments";
            }
            else
            {
                string deptName = departmentService.GetDepartmentByCode(deptCode).departmentName;
                ViewBag.Dept = deptName;
            }

            deptCode = (deptCode == "-1") ? null : deptCode;
            List<ReportViewModel> vmList = reportService.GetApprovedRequisitionDetialsBasedCriteria("-1", "-1", deptCode, new int[] { year }, null);
                
            var results = (from vm in vmList
                           group vm by new { vm.Year, vm.Month } into g
                           select new { Year = g.Key.Year,
                                        Month = g.Key.Month,
                                        Sum = g.Sum(v => v.RequestQuantity),
                                        Amount = g.Sum(v => v.RequestQuantity * v.Cost) }).OrderBy(r => r.Month); ;

            ViewBag.XLabels = months;

            // Get data for bar chart
            ViewBag.YBarLabel = "Monthly Quantity";
            int[] monthlyQuantity = new int[12];
            foreach (var r in results)
            {
                monthlyQuantity[r.Month - 1] = r.Sum;
            }
            ViewBag.YBarData = monthlyQuantity;

            // Get data for line
            ViewBag.YLineLabel = "Cumulative Amount";
            decimal[] cumulativeAmount = new decimal[12];
            decimal cumulative = 0.00M;
            foreach (var r in results)
            {
                cumulative = Math.Round((cumulative + r.Amount), 2);
                cumulativeAmount[r.Month - 1] = cumulative;
            }
            for(int i = 1; i < cumulativeAmount.Length; i++) // to let zero amount same as previous month
            {
                if(cumulativeAmount[i] == 0)
                {
                    cumulativeAmount[i] = cumulativeAmount[i - 1];
                }
            }
            ViewBag.YLineData = cumulativeAmount;

            return PartialView("_RequisitionCumulativeBar");
        }


        [HttpGet]
        public ActionResult ReorderAmountCumulativeChart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetReorderAmountCumulativeBar(int year, string supplierCode)
        {
            ViewBag.Year = year.ToString();

            if (supplierCode == "-1")
            {
                ViewBag.Supplier = "All Suppliers";
            }
            else
            {
                ViewBag.Supplier = supplierCode;
            }

            supplierCode = (supplierCode == "-1") ? null : supplierCode;
            List<ReportViewModel> vmList = reportService.GetReorderAmountBasedOnCriteria("-1", "-1", supplierCode, new int[] { year }, null);

            var results = (from vm in vmList
                           group vm by new { vm.Year, vm.Month } into g
                           select new
                           {
                               Year = g.Key.Year,
                               Month = g.Key.Month,
                               Sum = g.Sum(v => v.OrderQuantity),
                               Amount = g.Sum(v => v.OrderQuantity * v.Cost)
                           }).OrderBy(r => r.Month); ;

            ViewBag.XLabels = months;

            // Get data for bar chart
            ViewBag.YBarLabel = "Reorder Quantity";
            int[] monthlyQuantity = new int[12];
            foreach (var r in results)
            {
                monthlyQuantity[r.Month - 1] = r.Sum;
            }
            ViewBag.YBarData = monthlyQuantity;

            // Get data for line
            ViewBag.YLineLabel = "Cumulative Amount";
            decimal[] cumulativeAmount = new decimal[12];
            decimal cumulative = 0.00M;
            foreach (var r in results)
            {
                cumulative = Math.Round((cumulative + r.Amount), 2);
                cumulativeAmount[r.Month - 1] = cumulative;
            }
            for (int i = 1; i < cumulativeAmount.Length; i++) // to let zero amount same as previous month
            {
                if (cumulativeAmount[i] == 0)
                {
                    cumulativeAmount[i] = cumulativeAmount[i - 1];
                }
            }
            ViewBag.YLineData = cumulativeAmount;

            return PartialView("_ReorderAmountCumulativeBar");
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


            if(categoryID != "-1") // prepare display title for chart
            {
                ViewBag.Category = stationeryService.GetAllCategory().Find(x => x.categoryID.ToString() == categoryID).categoryName;
            }
            else
            {
                ViewBag.Category = "";
            }

            if(itemCode != "-1") // prepare display title for chart
            {
                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
                ViewBag.ItemCode = String.Format("{0} ({1})", itemCode, s.description);            
            }
            else
            {
                ViewBag.ItemCode = "All Items";
            }


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
        public ActionResult DeptMonthlyRequisitionAmount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDeptMonthlyRequisitionAmount(int year, int month = -1)
        {
            ViewBag.Year = year.ToString();
            ViewBag.Month = (month == -1) ? "" : months[month - 1]; // get string representation of months


            int[] m = (month == -1) ? null : new int[] { month }; // -1 for all months
            List<ReportViewModel> vmList = reportService.GetApprovedRequisitionDetialsBasedCriteria("-1", "-1", null, new int[] { year }, m);

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

            return PartialView("_DeptMonthlyRequisition");
        }


        [HttpGet]
        public ActionResult SupplierMonthlyReorderAmount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSupplierMonthlyReorderAmount(int year, int month = -1)
        {
            ViewBag.Year = year.ToString();
            ViewBag.Month = (month == -1) ? "" : months[month - 1]; // get string representation of months


            int[] m = (month == -1) ? null : new int[] { month }; // -1 for all months
            List<ReportViewModel> vmList = reportService.GetReorderAmountBasedOnCriteria("-1", "-1", null, new int[] { year }, m);

            var results = (from vm in vmList
                           group vm by vm.Supplier into g
                           select new { Supplier = g.Key, Cost = g.Sum(v => v.OrderQuantity * v.Cost) });

            List<string> supplier = new List<string>();
            List<decimal> cost = new List<decimal>();

            foreach (var r in results)
            {
                supplier.Add(r.Supplier);
            }

            supplier.Sort(); // fix the position of department showing on chart

            foreach (string s in supplier)
            {
                foreach (var r in results)
                {
                    if (r.Supplier == s)
                    {
                        cost.Add(Math.Round(r.Cost, 2));
                        break;
                    }
                }
            }


            ViewBag.XLabels = supplier.ToArray();
            ViewBag.YBarData = cost.ToArray();

            return PartialView("_SupplierMonthlyReorder");
        }



        [HttpGet]
        public ActionResult CategoryMonthlyRequisitionAmount()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetCategoryMonthlyRequisitionAmountDoughnutChart(int year, int month, string deptCode = "-1")
        {
            ViewBag.Year = year.ToString();
            ViewBag.Month = (month == -1) ? "" : months[month - 1]; // get string representation of months

            if (deptCode == "-1")
            {
                ViewBag.Dept = "All Departments";
            }
            else
            {
                string deptName = departmentService.GetDepartmentByCode(deptCode).departmentName;
                ViewBag.Dept = deptName;
            }

            deptCode = (deptCode == "-1") ? null : deptCode; // null to get all department
            List<ReportViewModel> vmList = reportService.GetApprovedRequisitionDetialsBasedCriteria("-1", "-1", deptCode, new int[] { year }, new int[] { month });
                
            var results = (from vm in vmList
                           group vm by vm.CategoryName into g
                           select new { Category = g.Key, Cost = g.Sum(v => v.RequestQuantity * v.Cost) });

            List<string> categories = new List<string>();
            List<decimal> cost = new List<decimal>();

            foreach (var r in results)
            {
                categories.Add(r.Category);
            }

            categories.Sort(); // fix the position of category showing on chart

            foreach (string c in categories)
            {
                foreach (var r in results)
                {
                    if (r.Category == c)
                    {
                        cost.Add(Math.Round(r.Cost, 2));
                        break;
                    }
                }
            }

            ViewBag.XLabels = categories.ToArray();
            ViewBag.YDoughnutData = cost.ToArray();

            return PartialView("_CategoryMonthlyRequisition");
        }

        [HttpGet]
        public ActionResult CategoryMonthlyReorderAmount()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetCategoryMonthlyReorderAmountDoughnutChart(int year, int month, string supplierCode = "-1")
        {
            ViewBag.Year = year.ToString();
            ViewBag.Month = (month == -1) ? "" : months[month - 1]; // get string representation of months

            if (supplierCode == "-1")
            {
                ViewBag.Supplier = "All Suppliers";
            }
            else
            {
                ViewBag.Supplier = supplierCode;
            }

            int[] m = (month == -1)? null : new int[] { month };
            supplierCode = (supplierCode == "-1") ? null : supplierCode; // null to get all 
            List<ReportViewModel> vmList = reportService.GetReorderAmountBasedOnCriteria("-1", "-1", supplierCode, new int[] { year }, m);

            var results = (from vm in vmList
                           group vm by vm.CategoryName into g
                           select new { Category = g.Key, Cost = g.Sum(v => v.OrderQuantity * v.Cost) });

            List<string> categories = new List<string>();
            List<decimal> cost = new List<decimal>();

            foreach (var r in results)
            {
                categories.Add(r.Category);
            }

            categories.Sort(); // fix the position of category showing on chart

            foreach (string c in categories)
            {
                foreach (var r in results)
                {
                    if (r.Category == c)
                    {
                        cost.Add(Math.Round(r.Cost, 2));
                        break;
                    }
                }
            }

            ViewBag.XLabels = categories.ToArray();
            ViewBag.YDoughnutData = cost.ToArray();

            return PartialView("_CategoryMonthlyReorder");
        }

        public JsonResult GetCategoryListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Category> categories = stationeryService.GetAllCategory();

            options.Add(new JSONForCombobox("-1", "All"));

            foreach (var c in categories)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = c.categoryID.ToString();
                option.text = c.categoryName;
                options.Add(option);
            }

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                options = (from o in options
                           where o.text.ToString().ToLower().Contains(term) || o.id.ToString().ToLower().Contains(term)
                           select o).ToList();
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStationeryListBasedOnCategoryJSON(string categoryID, string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(null, categoryID);

            if (categoryID != "-1") 
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


            if (!String.IsNullOrEmpty(term))  // used for select2 combobox filtering search result
            {
                string[] searchStringArray = term.Split();
                foreach (string s in searchStringArray)
                {
                    string search = s.ToLower().Trim();
                    if (!String.IsNullOrEmpty(search))
                    {
                        options = options.Where(x => (x.text.ToLower().Contains(search) || x.id.ToLower().Contains(search))).ToList();
                    }
                }
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectableYearsJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            int baseYear = reportService.GetEarliestYear();
            List<int> years = reportService.GetSelectableYears(baseYear);


            foreach (var y in years)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = y.ToString();
                option.text = y.ToString();
                options.Add(option);
            }

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                options = (from o in options
                           where o.text.ToString().ToLower().Contains(term) || o.id.ToString().ToLower().Contains(term)
                           select o).ToList();
            }


            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectableMonthsJSON(int year, string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<int> selectableMonths = reportService.GetSelectableMonths(year);

            options.Add(new JSONForCombobox("-1", "All"));


            foreach (var m in selectableMonths)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = m.ToString();
                option.text = months[m - 1];
                options.Add(option);
            }

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                options = (from o in options
                           where o.text.ToString().ToLower().Contains(term) || o.id.ToString().ToLower() == term
                           select o).ToList();
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Department> departments = departmentService.GetAllDepartment();

            options.Add(new JSONForCombobox("-1", "All"));
       
            foreach (var d in departments)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = d.departmentCode.ToString();
                option.text = d.departmentName;
                options.Add(option);
            }

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                options = (from o in options
                           where o.text.ToString().ToLower().Contains(term) || o.id.ToString().ToLower().Contains(term)
                           select o).ToList();
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupplierListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Supplier> suppliers = supplierService.GetSupplierList();

            options.Add(new JSONForCombobox("-1", "All"));

            foreach (var s in suppliers)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = s.supplierCode;
                option.text = s.supplierName;
                options.Add(option);
            }

            if (!String.IsNullOrEmpty(term))
            {
                // used for select2 combobox filtering search result
                options = (from o in options
                           where o.text.ToString().ToLower().Contains(term) || o.id.ToString().ToLower().Contains(term)
                           select o).ToList();
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }




        // TODO - REMOVE THIS METHOD 
        //public ActionResult GenerateRandomDataForRequisitionRecords()
        //{
        //    reportService.GenerateRandomDataForRequisitionRecords();
        //    return RedirectToAction("Login", "Home");
        //}

        // TODO - REMOVE THIS METHOD 
        //public ActionResult GenerateRandomDataForPurchaseRecords()
        //{
        //    reportService.GenerateRandomDataForPurchaseRecords();
        //    return RedirectToAction("Login", "Home");
        //}
    }
}

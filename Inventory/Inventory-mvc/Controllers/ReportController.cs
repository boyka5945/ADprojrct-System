using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.DAO; // remove
using Inventory_mvc.ViewModel;


namespace Inventory_mvc.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Report()
        {
            ChartDAO dao = new ChartDAO();
            List<ReportViewModel> vmList = dao.RetrieveQty();

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

            List<string> categories = new List<string>();
            List<int> quantity = new List<int>();

            foreach (var item in result)
            {
                categories.Add(item.CategoryName);
                quantity.Add(item.Qty);
            }

            string[] cat = categories.ToArray();
            int[] qty = quantity.ToArray();

            ViewBag.X = cat;
            ViewBag.Y = qty;

            TempData["x"] = string.Join(",", categories);
            TempData["Y"] = string.Join(",", quantity);
            return View();
        }

      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDepartment()
        {
            DepartmentService ds = new DepartmentService();
            List<Department> model = ds.GetAllDepartment();
            return View(model);
        }
    }
}
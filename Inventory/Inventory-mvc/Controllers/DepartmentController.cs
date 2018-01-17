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

        [HttpGet]
        public ActionResult EditDepartment(string deptCode)
        {
            DepartmentService ds = new DepartmentService();
            Department model = new Department();
            model = ds.GetDepartmentByCode(deptCode);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditDepartment(Department dept)
        {
            DepartmentService ds = new DepartmentService();
            int row = ds.UpdateDepartmentByCode(dept);


            return RedirectToAction("ListDepartment");
        }

        [HttpGet]
        public ActionResult CreateDepartment()
        {
            DepartmentService ds = new DepartmentService();
            Department model = new Department();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDepartment(Department dept)
        {
            DepartmentService ds = new DepartmentService();
            Boolean b = ds.CreateDepartment(dept);
            List<Department> model = ds.GetAllDepartment();
            return RedirectToAction("ListDepartment");
        }
    }
}
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

        IDepartmentService departmentService = new DepartmentService();
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
            TempData["sss"] = dept.departmentCode.ToString();
            string departmentCode = dept.departmentCode;
            DepartmentService ds = new DepartmentService();
            
            if (ModelState.IsValid)
            {
                try
                {
                    int row = ds.UpdateDepartmentByCode(dept);
                    
                    return RedirectToAction("ListDepartment");
                   
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }
            return View();
            
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
            string deptCode = dept.departmentCode;

            if (departmentService.isExistingCode(deptCode))
            {
                string errorMessage = String.Format("{0} has been used.", deptCode);
                ModelState.AddModelError("departmentCode", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    Boolean b = ds.CreateDepartment(dept);
                    List<Department> model = ds.GetAllDepartment();
                    return RedirectToAction("ListDepartment");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            return View(dept);

            
        }
    }
}
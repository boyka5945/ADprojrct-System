using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class DepartmentController : Controller
    {

        IDepartmentService departmentService = new DepartmentService();
        ICollectionPointService collectionPointService = new CollectionPointService();
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDepartment(int? page)
        {
            DepartmentService ds = new DepartmentService();
            List<Department> model = ds.GetAllDepartment();
            

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public ActionResult EditDepartment(string deptCode)
        {
            DepartmentService ds = new DepartmentService();
            Department model = new Department();

            model = ds.GetDepartmentByCode(deptCode);
            
            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
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
            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
            return View();

            
        }

        [HttpGet]
        public ActionResult CreateDepartment()
        {
            //DepartmentService ds = new DepartmentService();
            //Department model = new Department();

            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
            
            return View();
        }

        [HttpPost]
        public ActionResult CreateDepartment(Department dept)
        {
            var PointID = Convert.ToInt32( Request["collectionPointID"]);
            DepartmentService ds = new DepartmentService();
            dept.collectionPointID = PointID;

            string deptCode = dept.departmentCode;

            if (departmentService.isExistingCode(deptCode))
            {
                string errorMessage = String.Format("{0} has been used.", deptCode);
                ModelState.AddModelError("departmentCode", errorMessage);
            }
            else 
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

            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();


            return View();


        }
    }
}
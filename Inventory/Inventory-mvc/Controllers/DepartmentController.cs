using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using PagedList;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class DepartmentController : Controller
    {

        IDepartmentService departmentService = new DepartmentService();
        ICollectionPointService collectionPointService = new CollectionPointService();
        IUserService userService = new UserService();

        // GET: Department

        [RoleAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize]
        //ACCESS BY Store Clerk, Store Manager, Store Supervisor
        public ActionResult ListDepartment(int? page)
        {
            DepartmentService ds = new DepartmentService();
            List<Department> model = ds.GetAllDepartment();

            string userID = HttpContext.User.Identity.Name;
            ViewBag.RoleID = userService.GetRoleByID(userID);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));

        }

        [RoleAuthorize]
        //ACCESS BY Store Manager, Store Supervisor
        [HttpGet]
        public ActionResult EditDepartment(string deptCode)
        {
            DepartmentService ds = new DepartmentService();
            Department model = new Department();

            model = ds.GetDepartmentByCode(deptCode);
            
            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
            return View(model);
        }

        [RoleAuthorize]
        //ACCESS BY Store Manager, Store Supervisor
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

        [RoleAuthorize]
        //ACCESS BY Store Manager, Store Supervisor
        [HttpGet]
        public ActionResult CreateDepartment()
        {
            //DepartmentService ds = new DepartmentService();
            //Department model = new Department();

            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
            
            return View();
        }

        [RoleAuthorize]
        //ACCESS BY Store Manager, Store Supervisor
        [HttpPost]
        public ActionResult CreateDepartment(Department dept, FormCollection form)
        {
            DepartmentService ds = new DepartmentService();
            ViewBag.CollectionPointList = collectionPointService.GetAllCollectionPoints();
            try
            {
                var PointID = Convert.ToInt32(form["PointID"]);
                
                dept.collectionPointID = PointID;
            }
            catch
            {
                string errorMessage = String.Format("Select Collection Point.");
                ModelState.AddModelError("collectionPointID", errorMessage);
                return View();
            }        
           
            

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
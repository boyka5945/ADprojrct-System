using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class CollectionPointController : Controller
    {
        ICollectionPointService collectionPointService = new CollectionPointService();
        IDepartmentService departmentService = new DepartmentService();
        IUserService us = new UserService();
        IStationeryService ss = new StationeryService();
        IRequisitionRecordService rs = new RequisitionRecordService();


        // GET: CollectionPoint
        // *********ACCESS LEVEL: Store Manager
        [RoleAuthorize]
        public ActionResult Index()
        {
            return View(collectionPointService.GetAllCollectionPoints());
        }

        [RoleAuthorize]
        // GET: CollectionPoint/Create
        // *********ACCESS LEVEL: Store Manager********
        public ActionResult Create()
        {
            return View(new CollectionPointViewModel());
        }

        [RoleAuthorize]
        // POST: CollectionPoint/Create
        // *********ACCESS LEVEL: Store Manager*******
        [HttpPost]
        public ActionResult Create(CollectionPointViewModel collectionPointVM)
        {
            int id = Convert.ToInt32(collectionPointVM.collectionPointID);

            if (collectionPointService.isExistingCode(id))
            {
                string errorMessage = String.Format("{0} has been used.", id);
                ModelState.AddModelError("collectionPointID", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    collectionPointService.AddNewCollectionPoint(collectionPointVM);
                    TempData["CreateMessage"] = String.Format("Collection Point '{0}' is added.", id);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            return View(collectionPointVM);
        }

        [RoleAuthorize]
        // GET: CollectionPoint/Edit/{id}
        // *********ACCESS LEVEL: Store Manager********
        public ActionResult Edit(int id)
        {
            CollectionPointViewModel cpVM = collectionPointService.GetCollectionPointByID(id);
            return View(cpVM);
        }

        [RoleAuthorize]
        // POST: CollectionPoint/Edit/{id}
        // *********ACCESS LEVEL: Store Manager********
        [HttpPost]
        public ActionResult Edit(CollectionPointViewModel cpVM)
        {
            int id = cpVM.collectionPointID;

            if (ModelState.IsValid)
            {
                try
                {
                    if (collectionPointService.UpdateCollectionPointInfo(cpVM))
                    {
                        TempData["EditMessage"] = String.Format("'{0}' has been updated", id);
                    }
                    else
                    {
                        TempData["EditErrorMessage"] = String.Format("There is no change to '{0}'.", id);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                }
            }

            return View(cpVM);
        }

        [RoleAuthorize]
        // GET: CollectionPoint/Delete/{id}
        // *********ACCESS LEVEL: Store Manager********
        public ActionResult Delete(int id)
        {
            try
            {
                if (collectionPointService.DeleteCollectionPoint(id))
                {
                    TempData["DeleteMessage"] = String.Format("Collection Point '{0}' has been deleted", id);
                }
                else
                {
                    TempData["DeleteErrorMessage"] = String.Format("Cannot delete collection point '{0}'", id);
                }

               
            }
            catch(Exception e)
            {
                TempData["DeleteErrorMessage"] = String.Format("Cannot delete collection point '{0}'", id);
              
            }
            return RedirectToAction("Index");
        }



        //    GET: CollectionPoint/UpdateCollectionPoint/{department}
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [RoleAuthorize]
        public ActionResult UpdateCollectionPoint()
        {
            string userID = HttpContext.User.Identity.Name;

            DepartmentService ds = new DepartmentService();
            TempData["CollectionPointList"] = collectionPointService.GetAllCollectionPoints();
            Department uVM = ds.GetDepartmentByCode(us.FindByUserID(userID).departmentCode);
            return View(uVM);
        }

        [RoleAuthorize]
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [HttpPost]
        public ActionResult UpdateCollectionPoint(FormCollection form)
        {

            DepartmentService ds = new DepartmentService();
            var collectionPoint = Convert.ToInt32(form["collectionPointID"]);
            var departmentCode = form["departmentCode"].ToString();
            Department d = new Department();
            d = ds.GetDepartmentByCode(departmentCode);
            d.collectionPointID = collectionPoint;

            

            if (ModelState.IsValid)
                try
                {
                    int row = ds.UpdateDepartmentByCode(d);                                   
                    return RedirectToAction("UpdateCollectionPoint");

                }
                catch (EmailException e)
                {
                    TempData["ExceptionMessage"] = "Failure to send email notification. Kindly contact IT personnel.";
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }

            TempData["CollectionPointList"] = collectionPointService.GetAllCollectionPoints();
            return RedirectToAction("UpdateCollectionPoint");
        }

        [RoleAuthorize]
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [HttpGet]
        public ActionResult Collect_Item(int? page)
        {
            StationeryModel entity = new StationeryModel();
            string name = HttpContext.User.Identity.Name;
            string deptCode = us.FindDeptCodeByID(name);
            var deptName = entity.Departments.Where(x => x.departmentCode == deptCode).First().departmentName;
            var rep = entity.Users.Where(x => x.departmentCode == deptCode).Where(x=> x.role == 4||x.role==3||x.role==7).First().name;
            
            ViewBag.deptName = deptName;
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = departmentService.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;

            list = rs.GetRequisitionByDept(deptCode);
            
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = rep;
                    break;
                }
            }

            List<Requisition_Detail> list2 = rs.GetAllRequisitionByDept(deptCode);

            TempData["requisitionListbyrequest"] = list2;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [HttpGet]
        public PartialViewResult Collect_Item2(int id)
        {
            var partialViewModel = new PartialViewResult();
            
            StationeryModel entity = new StationeryModel();
            string name = HttpContext.User.Identity.Name;
            string deptCode = us.FindDeptCodeByID(name);
            var deptName = entity.Departments.Where(x => x.departmentCode == deptCode).First().departmentName;
            var rep = entity.Users.Where(x => x.departmentCode == deptCode).Where(x => x.role == 4 || x.role == 3 || x.role == 7).First().name;
            ViewBag.deptName = deptName;
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = rep;
                    break;
                }
            }
            
            
            if (id == 1)
            {
                List<Disbursement> list = new List<Disbursement>();
                list = rs.GetRequisitionByDept(deptCode);
                TempData["requisitionListbyitem"] = list;
                
                return PartialView("Collect_ItemByItem");
            }
            else if (id == 2)
            {
                List<Requisition_Detail> list = new List<Requisition_Detail>();
                list = rs.GetAllRequisitionByDept(deptCode);
                TempData["requisitionListbyrequest"] = list;
                
                return PartialView("Collect_ItemByRequest");
            }

            return null;
        }

        [RoleAuthorize]
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [HttpGet]
        public ActionResult Pending_Item(int? page)
        {
            StationeryModel entity = new StationeryModel();
            string name = HttpContext.User.Identity.Name;
            string deptCode = us.FindDeptCodeByID(name);
            var deptName = entity.Departments.Where(x => x.departmentCode == deptCode).First().departmentName;
            var rep = entity.Users.Where(x => x.departmentCode == deptCode).Where(x => x.role == 4 || x.role == 3 || x.role == 7).First().name;

            ViewBag.deptName = deptName;
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = departmentService.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;

            list = rs.GetPendingDisbursementByDept(deptCode);
            
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = rep;
                    break;
                }
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        // *********ACCESS LEVEL: Dept Head/Acting Dept Head/User Representative/Store Manager/********
        [HttpGet]
        public PartialViewResult Pending_Item2(int id)
        {

            var partialViewModel = new PartialViewResult();

            StationeryModel entity = new StationeryModel();
            string name = HttpContext.User.Identity.Name;
            string deptCode = us.FindDeptCodeByID(name);
            var deptName = entity.Departments.Where(x => x.departmentCode == deptCode).First().departmentName;
            var rep = entity.Users.Where(x => x.departmentCode == deptCode).Where(x => x.role == 4 || x.role == 3 || x.role == 7).First().name;

            ViewBag.deptName = deptName;
           
            
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = rep;
                    break;
                }
            }
          

            if (id == 1)
            {
                List<Disbursement> list = new List<Disbursement>();
                list = rs.GetPendingDisbursementByDept(deptCode);
                TempData["requisitionListbyitem"] = list;

                return PartialView("Pending_ItemByItem");
            }
            else if (id == 2)
            {
                List<Requisition_Detail> list = new List<Requisition_Detail>();
                list = rs.GetAllPendingDisbursementByDept(deptCode);
                TempData["requisitionListbyrequest"] = list;

                return PartialView("Pending_ItemByRequest");
            }

            return null;
                     
        }
    }
}


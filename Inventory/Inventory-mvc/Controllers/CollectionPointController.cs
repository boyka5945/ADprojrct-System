using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;

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
        public ActionResult Index()
        {
            return View(collectionPointService.GetAllCollectionPoints());
        }

        //public ActionResult ListCollectionPoint()
        //{
        //    CollectionPointService ds = new CollectionPointService();
        //    List<Collection_Point> model = ds.GetAllCollectionPoint();
        //    return View(model);
        //}

        // GET: CollectionPoint/Create
        public ActionResult Create()
        {
            return View(new CollectionPointViewModel());
        }

        // POST: CollectionPoint/Create
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

        // GET: CollectionPoint/Edit/{id}
        public ActionResult Edit(int id)
        {
            CollectionPointViewModel cpVM = collectionPointService.GetCollectionPointByID(id);
            return View(cpVM);
        }


        // POST: CollectionPoint/Edit/{id}
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
                        TempData["EditErrorMessage"] = String.Format("There is not change to '{0}'.", id);
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

        // GET: CollectionPoint/Delete/{id}
        public ActionResult Delete(int id)
        {
            if (collectionPointService.DeleteCollectionPoint(id))
            {
                TempData["DeleteMessage"] = String.Format("Supplier '{0}' has been deleted", id);
            }
            else
            {
                TempData["DeleteErrorMessage"] = String.Format("Cannot delete supplier '{0}'", id);
            }

            return RedirectToAction("Index");
        }



        //    GET: CollectionPoint/UpdateCollectionPoint/{department}

        public ActionResult UpdateCollectionPoint()
        {
            //hardcoded value before login being implemented
            string userID = "S1000";
            DepartmentService ds = new DepartmentService();
            TempData["CollectionPointList"] = collectionPointService.GetAllCollectionPoints();
            Department uVM = ds.GetDepartmentByCode(us.FindByUserID(userID).departmentCode);
            //string FindDeptCode = uVM.DepartmentCode;
            //DepartmentService ds = new DepartmentService();
            //Department d = ds.GetDepartmentByCode(FindDeptCode);
            return View(uVM);

        }


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
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }

            TempData["CollectionPointList"] = collectionPointService.GetAllCollectionPoints();
            return RedirectToAction("UpdateCollectionPoint");


        }

        [HttpGet]
        public ActionResult Collect_Item(int? page)
        {

            string deptCode = "ZOOL";
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = departmentService.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;

            list = rs.GetRequisitionByDept(deptCode);
            StationeryModel entity = new StationeryModel();
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = user.name;
                    break;
                }
            }
            

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Pending_Item(int? page)
        {

            string deptCode = "ZOOL";
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = departmentService.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;

            list = rs.GetRequisitionByDept(deptCode);
            StationeryModel entity = new StationeryModel();
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in departmentService.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = user.name;
                    break;
                }
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
    }
}


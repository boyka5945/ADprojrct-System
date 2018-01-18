using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Controllers
{
    public class CollectionPointController : Controller
    {
        ICollectionPointService collectionPointService = new CollectionPointService();
        IDepartmentService departmentService = new DepartmentService();
        IUserService us = new UserService();
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

        //public ActionResult UpdateCollectionPoint()
        //{
        //    //hardcoded value before login being implemented
        //    string userID = "S1000";
        //    DepartmentService ds = new DepartmentService();
        //    TempData["CollectionPointList"] = collectionPointService.GetAllCollectionPoints();
            
        //    Department uVM = ds.GetDepartmentByCode();
        //    //string FindDeptCode = uVM.DepartmentCode;
        //    //DepartmentService ds = new DepartmentService();
        //    //Department d = ds.GetDepartmentByCode(FindDeptCode);
        //    return View(uVM);
            
        //}


        //[HttpPost]
        //public ActionResult UpdateCollectionPoint([Bind(Include = "collectionPointID")] Collection_Point cp, [Bind(Include = "deptCode")] Department dp)
        //{



        //    Department d = new Department();
           
        //    d.departmentCode = dp.departmentCode;

        //    DepartmentService ds = new DepartmentService();

        //    if (ModelState.IsValid)
        //    try
        //    {
        //        int row = ds.UpdateDepartmentByCode(d);

        //        return RedirectToAction("UpdateCollectionPoint");

        //    }
        //    catch (Exception e)
        //    {
        //        TempData["ExceptionMessage"] = e.Message;
        //    }
        
         
        //    return View();


        //}
    }
}


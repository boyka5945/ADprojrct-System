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
    public class SupplierController : Controller
    {
        ISupplierService supplierService = new SupplierService();

        // GET: Supplier
        public ActionResult Index()
        {
            return View(supplierService.GetAllSuppliers());
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View(new SupplierViewModel());
        }

        // POST: Supplier/Create
        [HttpPost]
        public ActionResult Create(SupplierViewModel supplierVM)
        {
            if (supplierService.isExistingCode(supplierVM.SupplierCode))
            {
                string errorMessage = String.Format("{0} has been used.", supplierVM.SupplierCode);
                ModelState.AddModelError("SupplierCode", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    supplierService.AddNewSupplier(supplierVM);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            return View(supplierVM);
        }


        // GET: Supplier/Edit/{id}
        public ActionResult Edit(string id)
        {
            SupplierViewModel supplierVM = supplierService.FindBySupplierCode(id);
            return View(supplierVM);
        }


        // POST: Supplier/Edit/{id}
        [HttpPost]
        public ActionResult Edit(SupplierViewModel supplierVM)
        {
            string code = supplierVM.SupplierCode;

            if (ModelState.IsValid)
            {
                try
                {
                    if (supplierService.UpdateSupplierInfo(supplierVM))
                    {
                        TempData["EditMessage"] = String.Format("'{0}' has been updated", code);
                    }
                    else
                    {
                        TempData["EditErrorMessage"] = String.Format("There is not change to '{0}'.", code);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                }
            }

            return View(supplierVM);
        }


        // GET: Supplier/Delete/{id}
        public ActionResult Delete(string id)
        {
            if (supplierService.DeleteSupplier(id))
            {
                TempData["DeleteMessage"] = String.Format("Supplier '{0}' has been deleted", id);
            }
            else
            {
                TempData["DeleteErrorMessage"] = String.Format("Cannot delete supplier '{0}'", id);
            }

            return RedirectToAction("Index");
        }




    }
}
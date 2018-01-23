using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Controllers
{
    public class SupplierController : Controller
    {
        ISupplierService supplierService = new SupplierService();
        IUserService userService = new UserService();

        // GET: Supplier
        public ActionResult Index()
        {
            string userID = HttpContext.User.Identity.Name;

            // Store clerk roleID == 7
            ViewBag.RoleID = userService.GetRoleByID(userID);

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
            string code = supplierVM.SupplierCode.ToUpper().Trim();
            supplierVM.SupplierCode = code;

            if (supplierService.isExistingCode(code))
            {
                string errorMessage = String.Format("{0} has been used.", code);
                ModelState.AddModelError("SupplierCode", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    supplierService.AddNewSupplier(supplierVM);
                    TempData["SuccessMessage"] = String.Format("Supplier '{0}' is added.", code);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

            return View(supplierVM);
        }


        // GET: Supplier/Edit/{id}
        public ActionResult Edit(string id)
        {
            if(String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

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
                        TempData["SuccessMessage"] = String.Format("'{0}' has been updated", code);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = String.Format("There is not change to '{0}'.", code);
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
            if(String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            if (supplierService.DeleteSupplier(id))
            {
                TempData["SuccessMessage"] = String.Format("Supplier '{0}' has been deleted", id);
            }
            else
            {
                TempData["ErrorMessage"] = String.Format("Cannot delete supplier '{0}'", id);
            }

            return RedirectToAction("Index");
        }




    }
}
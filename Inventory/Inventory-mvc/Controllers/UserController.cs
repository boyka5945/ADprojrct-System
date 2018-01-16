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
    public class UserController : Controller
    {
        IUserService userService = new UserService();
        // GET: User
        public ActionResult UserList()
        {
            string name = HttpContext.User.Identity.Name;
            UserViewModel user = userService.FindByUserID("S1002");
            return View(userService.GetUserByDept(user));
        }
        [HttpGet]
        public ActionResult Delegate(string id)
        {
            UserViewModel u = userService.FindByUserID("S1002");
            return View(u);
        }
        [HttpPost]
        public ActionResult Delegate(string id, DateTime from, DateTime to)
        {
          //  DateTime fromd = DateTime.Parse(from);
            //DateTime tod = DateTime.Parse(to);
            userService.DelegateEmp(id, from, to);
            return RedirectToAction("UserList");
        }

        public ActionResult Edit(string id)
        {
            UserViewModel userVM = userService.FindByUserID(id);
            return View(userVM);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel userVM)
        {
            string code = userVM.UserID;


            if (ModelState.IsValid)
            {
                try
                {
                    if (userService.UpdateUserInfo(userVM))
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
            return View(userVM);
        }
    }
}
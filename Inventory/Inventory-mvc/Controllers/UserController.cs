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
            UserViewModel user = userService.FindByUserID("S1000");
            var model = userService.GetUserByDept(user);
            return View(model);
        }
        [HttpGet]
        public ActionResult Delegate(string id)
        {
            UserViewModel u = userService.FindByUserID(id);
            return View(u);
        }
        [HttpPost]
        public ActionResult Delegate()
        {            
            
            
                userService.DelegateEmp(Request["userID"].ToString(), Convert.ToDateTime(Request["from"]), Convert.ToDateTime(Request["toto"]));
            
            

            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult Remove_Delegate(string id)
        {
            userService.Remove_Delegate(id);
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
            string uid = userVM.UserID;


            if (ModelState.IsValid)
            {
                try
                {
                    if (userService.UpdateUserInfo(userVM))
                    {
                        TempData["EditMessage"] = String.Format("'{0}' has been updated", uid);
                    }
                    else
                    {
                        TempData["EditErrorMessage"] = String.Format("There is not change to '{0}'.", uid);
                    }

                    return RedirectToAction("UserList");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                }
            }
            return View(userVM);
        }

        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Create(UserViewModel userVM)
        {
            string id = userVM.UserID;

            if (userService.isExistingID(id))
            {
                string errorMessage = String.Format("{0} alrdady existed.", id);
                ModelState.AddModelError("UserID", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    userService.AddNewUser(userVM);
                    TempData["CreateMessage"] = String.Format("User '{0}' is added.", id);
                    return RedirectToAction("UserList");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            return View(userVM);
        }
        [HttpGet]
        public ActionResult Assign_Rep(string id)
        {
            int i = 0;
            List<string> roles = userService.FindAllRole();
            foreach(string r in roles)
            {
                if(r== "UserRepresentative")
                {
                    i++;
                }

            }
            if(i<1)
            {
                TempData["CannotAssignMessage"] = String.Format("Cannot assign two representative");
            }
             else
                {
                if (userService.AssignRep(id))
                {
                    TempData["AssignMessage"] = String.Format("'{0}' has been updated", id);
                }
                else
                {
                    TempData["AssignErrorMessage"] = String.Format("There is not change to '{0}'.", id);
                }
            }
            //string uid = Request["userID"].ToString();

            return RedirectToAction("UserList");
        }

        
    }
}
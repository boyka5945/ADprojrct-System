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
          //  ViewBag.Roles = userService.FindAllRole(user.UserID);
            //ViewBag.alrDelegated = userService.AlrDelegated(id);
            return View(model);
        }
        [HttpGet]
        public ActionResult Delegate(string id)
        {
            List<string> roles = userService.FindAllRole(id);
            foreach(string r in roles)
            {
                if (r=="ActingDeptHead")
                {
                    TempData["DelegateMessage"] = String.Format("Not allowed to delegate two employees");
                    return RedirectToAction("UserList");
                }
            }
            
            UserViewModel u = userService.FindByUserID(id);
            return View(u);
        }
        [HttpPost]
        public ActionResult Delegate()
        {
            DateTime start = Convert.ToDateTime(Request["from"]);
            DateTime end = Convert.ToDateTime(Request["toto"]);
            if(end.CompareTo(start)<1)
            {
                TempData["InvalidDateMessage"] = String.Format("Invalid end date");
                return View();
            }
            userService.DelegateEmp(Request["userID"].ToString(), start.Date , end.Date);

           
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult Remove_Delegate(string id)
        {
            if(userService.AlrDelegated(id))
            {
                userService.Remove_Delegate(id);
            }
            else
            {
                TempData["RmvDelMessage"] = String.Format("Employee is not delegated");
            }
           
            return RedirectToAction("UserList");

        }


        public ActionResult Edit(string id)
        {
            UserViewModel userVM = userService.FindByUserID(id);
            ViewBag.RoleList = userService.RoleForEditAndCreate(id);
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

        //public ActionResult Create()
        //{
            
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(UserViewModel userVM)
        //{
        //    string id = userVM.UserID;

        //    if (userService.isExistingID(id))
        //    {
        //        string errorMessage = String.Format("{0} alrdady existed.", id);
        //        ModelState.AddModelError("UserID", errorMessage);
        //    }
        //    else if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            userService.AddNewUser(userVM);
        //            TempData["CreateMessage"] = String.Format("User '{0}' is added.", id);
        //            return RedirectToAction("UserList");
        //        }
        //        catch (Exception e)
        //        {
        //            TempData["ExceptionMessage"] = e.Message;
        //        }
        //    }

        //    return View(userVM);
        //}
        [HttpGet]
        public ActionResult Assign_Rep(string id)
        {
           
                if (userService.AssignRep(id))
                {
                    TempData["AssignMessage"] = String.Format("'{0}' has been updated", id);
                }
                else
                {
                    TempData["AssignErrorMessage"] = String.Format("Cannot assign two representative");
                }
           
            //string uid = Request["userID"].ToString();

            return RedirectToAction("UserList");
        }

        
    }
}
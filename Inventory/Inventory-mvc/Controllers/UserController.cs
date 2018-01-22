using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System.Globalization;

namespace Inventory_mvc.Controllers
{
    public class UserController : Controller
    {
        IUserService userService = new UserService();
        // GET: User
        public ActionResult UserList()
        {
            string name = HttpContext.User.Identity.Name;
            //if(name=="")
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            User user = userService.FindByUserID("S1015");
            List<User> model = userService.GetUserByDept(user);
            userService.AutoRemove(user);
            
            return View(model);
        }

        public ActionResult SMUserList()
        {
            string name = HttpContext.User.Identity.Name;
            //if(name=="")
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            User user = userService.FindByUserID("S1015");
            List<User> model = userService.GetUserByDept(user);
            userService.AutoRemove(user);

            return View(model);
        }

        [HttpGet]
        public ActionResult Delegate(string id)
        {
            List<int> roles = userService.FindAllRole(id);
            foreach(int r in roles)
            {
                if (r==8)
                {
                    TempData["DelegateMessage"] = String.Format("Not allowed to delegate two employees");
                    return RedirectToAction("UserList");
                }
            }
            
            User u = userService.FindByUserID(id);
            return View(u);
        }
        [HttpPost]
        public ActionResult Delegate(string userID,string from, string toto )
        {           
            DateTime start = DateTime.ParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(toto, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            userService.DelegateEmp(Request["userID"].ToString(), start.Date, end.Date);

            HttpContext.Application["EndDate"] = end;

            User user = userService.FindByUserID(userID);
            if (user.departmentCode=="STORE")
            {
                return RedirectToAction("SMUserList");
            }      
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

            User user = userService.FindByUserID(id);
            if (user.departmentCode == "STORE")
            {
                return RedirectToAction("SMUserList");
            }
            return RedirectToAction("UserList");

        }


        public ActionResult Edit()
        {
            string name = HttpContext.User.Identity.Name;
            //if(name=="")
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            User user = userService.FindByUserID("S1015");
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User userVM)
        {
            string uid = userVM.userID;


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
            ViewBag.RoleList = userService.GetStoreRoles();
            return View(new User());
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            string id = user.userID;

            if (userService.isExistingID(id))
            {
                string errorMessage = String.Format("{0} alrdady existed.", id);
                ModelState.AddModelError("UserID", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    userService.AddNewUser(user);
                    TempData["CreateMessage"] = String.Format("User '{0}' is added.", id);
                    return RedirectToAction("UserList");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            ViewBag.RoleList = userService.GetStoreRoles();
            return RedirectToAction("SMUserList");
            //            return View(user);
        }
        [HttpGet]
        public ActionResult Assign_Rep(string id)
        {
           
                if (userService.AssignRep(id))
                {
                    TempData["AssignRepMessage"] = String.Format("'{0}' has been updated", id);
                }
                else
                {
                    TempData["AssignRepErrorMessage"] = String.Format("Cannot assign two representative");
                }
           
            //string uid = Request["userID"].ToString();

            return RedirectToAction("UserList");
        }

        
    }
}
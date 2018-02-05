using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System.Globalization;
using Inventory_mvc.Utilities;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class UserController : Controller
    {
        IUserService userService = new UserService();
        // GET: User
        [RoleAuthorize]
        public ActionResult UserList()   //DeptHead
        {
            string name = HttpContext.User.Identity.Name;
            //if(name=="")
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            User user = userService.FindByUserID(name);
            List<User> model = userService.GetUserByDept(user);
            ViewBag.Role = user.role;
            userService.AutoRemove(user);
            
            return View(model);
        }

        [RoleAuthorize]
        public ActionResult SMUserList()   //StoreManager
        {
            string name = HttpContext.User.Identity.Name;
            //if(name=="")
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            User user = userService.FindByUserID(name);
            List<User> model = userService.GetUserByDept(user);
            userService.AutoRemove(user);

            return View(model);
        }

        [RoleAuthorize]
        [HttpGet]
        public ActionResult Delegate(string id)    //DeptHead
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

        [RoleAuthorize]
        [HttpPost]
        public ActionResult Delegate(string userID,string from, string toto )  //DeptHead
        {
            string startDate = from;
            string endDate = toto;
                 
            if (startDate.Equals("") || endDate.Equals(null) || endDate.Equals(""))
            {
                User u = userService.FindByUserID(userID);
                TempData["NO_Date_ERROR_MESSAGE"] = String.Format("Please choose both start date and end date.");
                return View(u);
            }
            else
            {
                DateTime start = DateTime.ParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(toto, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                userService.DelegateEmp(Request["userID"].ToString(), start.Date, end.Date);

                HttpContext.Application["EndDate"] = end;

                User user = userService.FindByUserID(userID);
    
            }

            return RedirectToAction("UserList");
           
            
        }

        [RoleAuthorize]
        [HttpGet]
        public ActionResult Remove_Delegate(string id)  //DeptHead
        {
            if(userService.AlrDelegated(id))
            {
                userService.Remove_Delegate(id);
                TempData["RemoveDelegationdMessage"] = String.Format("Delegation Removed");
            }
            else
            {
                TempData["RmvDelMessage"] = String.Format("Employee is not delegated");
            }

            return RedirectToAction("UserList");

        }

        [RoleAuthorize]
        public ActionResult Edit()   //AllUsers
        {
            string name = HttpContext.User.Identity.Name;
            User user = userService.FindByUserID(name);
            return View(user);
        }

        [RoleAuthorize]
        [HttpPost]
        public ActionResult Edit(User userVM)  //AllUsers
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

        [RoleAuthorize]
        public ActionResult Create()  //StoreManager
        {
            ViewBag.RoleList = UserRoles.GetCreatableRolesForDepartment["Store"];
            User newUser = new User();
            newUser.departmentCode = "STORE";
            return View(newUser);
        }

        [RoleAuthorize]
        [HttpPost]
        public ActionResult Create(User user)    //StoreManager
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
                    return RedirectToAction("SMUserList");
                }
                catch (Exception e)
                {
                    TempData["ExceptionMessage"] = e.Message;
                }
            }

            ViewBag.RoleList = UserRoles.GetCreatableRolesForDepartment["Store"];
            //return RedirectToAction("SMUserList");
            return View(user);
        }

        [RoleAuthorize]
        [HttpGet]
        public ActionResult Assign_Rep(string id)   //DeptHead
        {
           
                if (userService.AssignRep(id))
                {
                    TempData["AssignRepMessage"] = String.Format("Employee already assigned as representative", id);
                }
                else
                {
                    TempData["AssignRepErrorMessage"] = String.Format("Cannot assign two representative");
                }
           

            return RedirectToAction("UserList");
        }


        
        [AllowAnonymous]
        public ActionResult ChangePassword()  //AllUsers
        {
            string userID = HttpContext.User.Identity.Name;
            User user = userService.FindByUserID(userID);
            ChangePasswordViewModel viewModel = userService.changePasswordUser(user);
            return View(viewModel);
        }

        [RoleAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordVM)  //AllUsers
        {
            ChangePasswordViewModel vm2 = changePasswordVM;
            string userID = HttpContext.User.Identity.Name;
            User user = userService.FindByUserID(userID);

            string oldPassword = changePasswordVM.OldPassword;
            string newPassword = changePasswordVM.NewPassword;
            string confirmPassword = changePasswordVM.ConfirmPassword;

            string password = user.password;
            string code = user.userID;
           
            
            if (ModelState.IsValid)
            {
                bool cond1 = userService.isSame(Encrypt.DecryptMethod(password), oldPassword);
                if (cond1)
                {
                    TempData["IncorrectPassword"] = String.Format("Incorrect Password.");
                }
                bool cond2 = userService.isSame(Encrypt.DecryptMethod(password), newPassword);
                if (!cond2)
                {
                    TempData["SameWithOldPassword"] = String.Format("Same With Password.");
                }
                if (newPassword!=confirmPassword)
                {
                    TempData["NotMatch"] = String.Format("New password and confirm password does not match");
                   
                }
                if(!cond1 && cond2 && newPassword == confirmPassword)
                {
                    try
                    {

                        bool t = userService.changePassword(changePasswordVM);
                        if (!t)
                        {
                            TempData["ErrorMessage"] = String.Format("There is not change to '{0}'.", code);
                            
                        }
                        else
                        {
                            TempData["SuccessMessage"] = String.Format("'{0}' has been updated", code);
                        }

                        return RedirectToAction("Edit");
                    }
                    catch (Exception e)
                    {
                        ViewBag.ExceptionMessage = e.Message;
                    }
                }

                //else
                //{ }
                return View(changePasswordVM); ;
            }
           
            
            return View(changePasswordVM);
            
        }

        [RoleAuthorize]
        public ActionResult Promote(string id)   //StoreManager
        {
            if (userService.Promote(id))
            {
                TempData["PromoteMessage"] = String.Format("'{0}' has been promoted as Store supervisor", id);
            }
            else
            {
                TempData["PromoteErrorMessage"] = String.Format("Cannot promote");
            }
            return RedirectToAction("SMUserList");
        }

        [RoleAuthorize]
        public ActionResult Demote(string id)   //StoreManager
        {
            if (userService.Demote(id))
            {
                TempData["DemoteMessage"] = String.Format("'{0}' has been demoted as Employee", id);
            }
            else
            {
                TempData["DemoteErrorMessage"] = String.Format("Cannot demote");
            }
            return RedirectToAction("SMUserList");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Inventory_mvc.Function;
using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Controllers
{
    public class HomeController : Controller
    {
        IUserService UserService = new UserService();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountLoginModels model, string returnUrl)
        {
            if (String.IsNullOrEmpty(model.UserName) || !UserService.isExistingID(model.UserName))
            {
                ViewBag.errorMessage = "Username is not correct.";
            }
            else
            {
                if (Encrypt.DecryptMethod(UserService.FindByUserID(model.UserName).password) == model.Password)
                {
                    int roleID = UserService.GetRoleByID(model.UserName);
                    string identity = model.UserName;
                    HttpContext.Application["role"] = roleID;
                    AuthorizationManager.SetTicket(Response, model.RememberMe, identity.ToUpper(), roleID);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.errorMessage = "Password is not correct.";

                }
            }

            return View(model);
        }

        //[RoleAuthorize]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            // TODO - CHANGE TO SESSION STATE
            //HttpContext.Application.Clear();
            HttpContext.Application["role"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }



    }
}
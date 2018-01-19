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
            var check = Request["checkV"].ToString();
            if (check == "BBB")
            {
                if (!UserService.isExistingID(model.UserName))
                {
                    ViewBag.errorMessage = "UserName or PassWord is not correct.";
                }
                else if (ModelState.IsValid)
                {
                    if (UserService.FindByUserID(model.UserName).Password == model.Password)
                    {
                        int roleID = 1;
                        string identity = model.UserName;
                        AuthorizationManager.SetTicket(Response, model.RememberMe, identity, roleID);
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else//null
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        [RoleAuthorize]
        public ActionResult Contact()
        {
            ViewBag.UserID = HttpContext.User.Identity.Name;
            return View();
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}
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
                ViewBag.errorMessage = "UserName or Password is not correct.";
            }
            else
            {
                if (Encrypt.DecryptMethod(UserService.FindByUserID(model.UserName).password) == model.Password)
                {
                    int roleID = UserService.GetRoleByID(model.UserName);
                    
                    string identity = model.UserName;
                    Session["role"] = roleID;
                    Session["Name"] = UserService.FindNameByID(identity);
                    StationeryModel entity = new StationeryModel();

                    Session["roleName"] = entity.Users.Where(x => x.userID == identity).First().roleInfo.roleName;
                    AuthorizationManager.SetTicket(Response, model.RememberMe, identity.ToUpper(), roleID);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else//null
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.errorMessage = "UserName or Password is not correct.";

                } 
            }

            return View(model);
        }

        public ActionResult Error()
        {
            return View();
        }

        [RoleAuthorize]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //HttpContext.Application.Clear();
            Session["role"] = null;
            Session["RequestList"] = null;
            Session["NewVoucher"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}
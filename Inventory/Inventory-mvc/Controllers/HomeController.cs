using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Function;
using Inventory_mvc.Models;


namespace Inventory_mvc.Controllers
{
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountLoginModels model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (model.UserName == "boyka")
                {
                    int roleID = 1;
                    string identity = model.UserName;
                    AuthorizationManager.SetTicket(Response, model.RememberMe, identity, roleID);
                }
                else if (model.UserName == "cyt")
                {
                    int roleID = 2;
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
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "page2";

            return View();
        }

        [RoleAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "page3";

            return View();
        }

        public ActionResult Email()
        {
            ViewBag.Message = "email have be sent successfully.";
            sendEmail email = new sendEmail("yellowtown940924@163.com", "nice to meet you", "hello");
            email.send();
            return View();
        }
    }
}
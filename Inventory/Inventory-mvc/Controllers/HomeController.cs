using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "page2";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "page3";

            return View();
        }

        public ActionResult Email()
        {
            ViewBag.Message = "email have be sent successfully.";
            sendEmail email = new sendEmail("yellowtown940924@163.com", "nice to meet you", "hello", @"C:\Users\yello\OneDrive\Desktop\like.txt");
            email.send();
            return View();
        }
    }
}
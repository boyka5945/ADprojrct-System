using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;

namespace Inventory_mvc.Controllers
{
    public class UserController : Controller
    {
        IUserService userService = new UserService();
        // GET: User
        public ActionResult UserList()
        {
            return View(userService.GetAllUser());
        }
    }
}
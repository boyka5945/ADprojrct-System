using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Function;
using Inventory_mvc.Entity;
using Inventory_mvc.Models;
using System.Reflection;

namespace Inventory_mvc.Controllers
{
    //init permission controller
    public class InstallerController : Controller
    {
        private static int permissionID = 0;
        public ActionResult Index()
        {
            var roleService = new RoleService();

            createPermission(new HomeController());

            //get all defined permission from db
            var allDefinedPermissions = roleService.GetDefinedPermissions();

            //init superAdmin
            var adminPermissions = new List<permissionInfo>();
            foreach (var d in allDefinedPermissions)
            {
                adminPermissions.Add(d);
            }
            roleService.AddRole(new RoleInfo
            {
                RoleId = 1,
                RoleName = "superAdmin",
                Description = "All permission reserved.",
                Permissions = adminPermissions
            });

            return RedirectToAction("Success");
        }
        private void createPermission(HomeController customController)
        {
            var roleService = new RoleService();

            var controller = "";
            var action = ""; 

            var controllerType = customController.GetType();
            controller = controllerType.Name.Replace("Controller", "");//remobe controller posfix
            //get action from a controller
            for (; permissionID < controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length; permissionID ++)
            {
                action = controllerType.GetMethods()[permissionID].Name;
                roleService.CreatePermissions(permissionID, controller, action);
            }
    
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
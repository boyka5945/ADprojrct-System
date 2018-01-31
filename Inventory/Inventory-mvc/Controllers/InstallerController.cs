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
        private static RoleService roleService = new RoleService();

        public ActionResult Index()
        { 
            createPermission(new List<Controller>{new HomeController(),
                                                  new AdjustmentVoucherController(),
                                                  new CheckShortfallController(),
                                                  new CollectionPointController(),
                                                  new DepartmentController(),
                                                  new InventoryCheckController(),
                                                  new ListRequisitionsController(),
                                                  new ManageRequisitionsController(),
                                                  new POGeneratorController(),
                                                  new PurchaseController(),
                                                  new RaiseRequisitionController(),
                                                  new ReceiveStockController(),
                                                  new ReportController(),
                                                  new StationeryController(),
                                                  new SupplierController(),
                                                  new UserController()
            });

            //get all defined permission from db
            var allDefinedPermissions = roleService.GetDefinedPermissions();

            //init superAdmin
            var adminPermissions = new List<permissionInfo>();
            foreach (var d in allDefinedPermissions)
            {
                adminPermissions.Add(d);
            }
            roleService.AddRole(new RoleInfos
            {
                RoleId = 1,
                RoleName = "SuperAdmin",
                Description = "All permission reserved.",
                Permissions = adminPermissions
            });

            return RedirectToAction("Success");
        }
        private static void createPermission(List<Controller> customController)
        {
            var controller = "";
            var action = "";
            foreach (var i in customController)
            {
                var controllerType = i.GetType();
                controller = controllerType.Name.Replace("Controller", "");//remobe controller posfix
                                                                           //get action from a controller
                int length = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length;
                var list = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                int add = permissionID;
                for (; permissionID < length + add; permissionID++)
                {
                    action = list[permissionID - add].Name;
                    roleService.CreatePermissions(permissionID, controller, action);
                }
            }
    
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
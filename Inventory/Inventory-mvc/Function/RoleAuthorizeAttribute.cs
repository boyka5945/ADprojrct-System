using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Inventory_mvc.Models;

namespace Inventory_mvc.Function
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuth = false;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                isAuth = false;
                
            }
            else
            {
                if (filterContext.RequestContext.HttpContext.User.Identity != null)
                {
                    //find controller and action requested
                    RoleService roleService = new RoleService();
                    var actionDescriptor = filterContext.ActionDescriptor;
                    var controllerDescriptor = actionDescriptor.ControllerDescriptor;
                    var controller = controllerDescriptor.ControllerName;
                    var action = actionDescriptor.ActionName;

                    //find ticket according to cookie identity
                    var ticket = (filterContext.RequestContext.HttpContext.User.Identity as FormsIdentity).Ticket;

                    //authorising rights to this user according roleID
                    var role = roleService.GetById(ticket.Version);
                    if (role != null)
                    {
                        isAuth = role.Permissions.Any(x => x.controller.ToLower() == controller.ToLower() && x.action.ToLower() == action.ToLower());
                    }
                }
            }
            if (!isAuth)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error", returnUrl = filterContext.HttpContext.Request.Url, returnMessage = "can not be accessiable." }));
                HttpContext.Current.Session["auth"] = isAuth;
                return;
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
            HttpContext.Current.Session["auth"] = isAuth;

        }
    }
}
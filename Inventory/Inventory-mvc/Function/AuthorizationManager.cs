using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Inventory_mvc.Function
{
        public class AuthorizationManager
        {
            public static void SetTicket(HttpResponseBase response, bool remeberMe, string identity, int role)
            {
                //create cookie
                FormsAuthentication.SetAuthCookie(identity, remeberMe);
                //create ticket, use ticket.version save roleID
                var authTicket = new FormsAuthenticationTicket(
                 role,
                 identity,
                 DateTime.Now, DateTime.Now.AddDays(30),
                 remeberMe,
                 identity);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(authTicket));
                response.Cookies.Add(authCookie);
            }

        }
    
}
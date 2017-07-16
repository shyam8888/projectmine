using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboUSER
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Admin",
            url: "Admin/{controller}/{action}/{id}",
            defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional },
             namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
            ).DataTokens = new RouteValueDictionary(new { area = "user" });


            routes.MapRoute(
           name: "Client_Home",
           url: "dashboard/client/{id}",
           defaults: new { controller = "dashboard", action = "client", id = UrlParameter.Optional },
            namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
           ).DataTokens = new RouteValueDictionary(new { area = "user" });

            routes.MapRoute(
        name: "Employee_Home",
        url: "dashboard/user/{id}",
        defaults: new { controller = "dashboard", action = "employee", id = UrlParameter.Optional },
         namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
        ).DataTokens = new RouteValueDictionary(new { area = "user" });
            routes.MapRoute(
      name: "Admin_Home",
      url: "dashboard/admin/{id}",
      defaults: new { controller = "dashboard", action = "admin", id = UrlParameter.Optional },
       namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
      ).DataTokens = new RouteValueDictionary(new { area = "user" });


            routes.MapRoute(
   name: "Employee_Registartion",
   url: "Register/{id}",
   defaults: new { controller = "Register", action = "Signup", id = UrlParameter.Optional },
    namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
   ).DataTokens = new RouteValueDictionary(new { area = "user" });
            routes.MapRoute(
name: "Registartion",
url: "Signup/{id}",
defaults: new { controller = "DashboardHome", action = "Register", id = UrlParameter.Optional },
namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
).DataTokens = new RouteValueDictionary(new { area = "user" });
            routes.MapRoute(
name: "ThankYou",
url: "thank-you/{id}",
defaults: new { controller = "DashboardHome", action = "Thankyou", id = UrlParameter.Optional },
namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
).DataTokens = new RouteValueDictionary(new { area = "user" });

            routes.MapRoute(
name: "LoadCity",
url: "registerform/loadcity/",
defaults: new { controller = "Register", action = "LoadCity", id = UrlParameter.Optional },
namespaces: new[] { "LaboUSER.Areas.user.Controllers" }
).DataTokens = new RouteValueDictionary(new { area = "user" });

            routes.MapRoute(
               name: "Checkouts-Create",
               url: "checkouts",
               defaults: new { controller = "Payment", action = "Create" },
               namespaces: new[] { "LaboUSER.Areas.user.Controllers" },
               constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) }
           ).DataTokens = new RouteValueDictionary(new { area = "user" });
            routes.MapRoute(
              name: "error-authorized",
              url: "not-authorized",
              defaults: new { controller = "error", action = "notfound" },
              namespaces: new[] { "LaboUSER.Areas.user.Controllers" },
              constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) }
          ).DataTokens = new RouteValueDictionary(new { area = "user" });

            routes.MapRoute(
             name: "login",
             url: "login",
             defaults: new { controller = "Login", action = "Login" },
             namespaces: new[] { "LaboUSER.Areas.user.Controllers" },
             constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) }
         ).DataTokens = new RouteValueDictionary(new { area = "user" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                namespaces: new[] { "LaboUSER.Controllers" },
                defaults: new { controller = "DashboardHome", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
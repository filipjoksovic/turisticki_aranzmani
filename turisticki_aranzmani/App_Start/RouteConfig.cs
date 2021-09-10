using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace turisticki_aranzmani
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "User/CreateManager",
                url: "createManager",
                new { controller = "User", action = "CreateManager" }
                );

            routes.MapRoute(
                name: "User/Admin",
                url: "admin",
                new { controller = "User", action = "Admin" }
                );

            routes.MapRoute(
                name: "User/Logout",
                url: "logout",
                new { controller = "User", action = "Logout" }
            );

            routes.MapRoute(
               name: "User/Login",
               url: "login",
               new { controller = "User", action = "Login" }
           );
            routes.MapRoute(
                name: "User/Register",
                url: "register",
                new { controller = "User", action = "Register" }
            );
            routes.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}

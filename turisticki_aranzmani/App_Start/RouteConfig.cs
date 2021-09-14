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
                name:"Reservation/Create",
                url:"createReservation",
                new {controller = "Reservation",action = "Create"}
                );
            routes.MapRoute(
                name: "Arrangement/View",
                url: "{role}/viewArrangements",
                new { controller = "Arrangement", action = "ViewArrangements" }
                );
            routes.MapRoute(
                name:"Arrangement/Details",
                url:"arrangementDetails/{id}",
                new {controller = "Arrangement", action = "Details"}
                );
            routes.MapRoute(
                name: "Arrangement/Create",
                url: "seller/createArrangement",
                new { controller = "Arrangement", action = "Create" }
                );
            routes.MapRoute(
                name : "ResidenceItem/Details/",
                url:"getResidenceItemDetails/{id}",
                new {controller = "ResidenceItem",action = "Details"}
                );
            routes.MapRoute(
                name: "ResidenceItem/CreateResidenceUnit/",
                url: "seller/createResidenceUnit/{id}",
                new { controller = "ResidenceItem", action = "CreateResidenceUnit" });

            routes.MapRoute(
                name: "ResidenceItem/ListResidenceUnits/",
                url: "seller/listResidenceUnits/{id}",
                new { controller = "ResidenceItem", action = "ListResidenceUnits" });

            routes.MapRoute(
                name: "Residence/ListResidences",
                url: "seller/listResidences/",
                new { controller = "Residence", action = "ListResidences" }
                );

            routes.MapRoute(
                name: "Residence/Create",
                url: "seller/createResidence",
                new { controller = "Residence", action = "Create" }
                );
            routes.MapRoute(
                name: "User/Create",
                url: "createUser",
                new { controller = "User", action = "Create" });
            routes.MapRoute(
                name: "User/CreateManager",
                url: "createManager",
                new { controller = "User", action = "CreateManager" }
                );
            routes.MapRoute(
                name: "User/Seller",
                url: "seller",
                new { controller = "User", action = "Seller" }
                );




            routes.MapRoute(
                name: "User/ViewUsers",
                url: "admin/viewUsers",
                new { controller = "User", action = "ViewUsers" }
                );
            routes.MapRoute(
                name: "User/Delete",
                url: "admin/deleteUser/{username}",
                new { controller = "User", action = "Delete" }
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

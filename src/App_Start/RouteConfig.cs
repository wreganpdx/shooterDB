using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication4
{
    public class RouteConfig
    {
        public static string ip = "dbclass.cs.pdx.edu";
        public static string connectString = "Server = " + RouteConfig.ip + "; Port=5432; User Id = w19wdb33; Password=XXXXXXXX; Database=w19wdb33;";
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Edit Shooter",
                url: "edit/shooter/{first}_{last}",
                defaults: new { controller = "Shooter", action = "EditShooter" });

            routes.MapRoute(
                name: "gunPurchaseId",
                url: "Guns/EditShooterGunPurchase/{gpId}",
                defaults: new { controller = "Home", action = "Index" });



            routes.MapRoute(
                name: "Search Results",
                url: "Home/SearchResults/{id}",
                defaults: new { controller = "Home", action = "SearchResults", id = UrlParameter.Optional});


            routes.MapRoute(
                name: "Basic Shooter Action",
                url: "shooter/{action}/{first}_{last}",
                defaults: new { controller = "Shooter", action = "CommitShooter" });

            routes.MapRoute(
                name: "Pages on Shooter",
                url: "{controller}/{action}/{first}_{last}/{page}",
                defaults: new { controller = "Shooter", action = "CommitShooter" });



            routes.MapRoute(
                name: "Basic Shooter Action 2",
                url: "edit/shooter/{action}",
                defaults: new { controller = "Shooter", action = "CommitShooter" });

            //changing shooter/action to higher priority fixed bugs related to submissions not routing properly.
            routes.MapRoute(
                         name: "Edit Shooter Action",
                         url: "shooter/{action}",
                         defaults: new { controller = "Shooter", action = "CommitShooter" });

            routes.MapRoute(
                name: "Gun Type Add",
                url: "Guns/GunTypeAdd/{gName}_{tId}",
                defaults: new { controller = "Home", action = "Index" });


            routes.MapRoute(
                name: "Default4",
                url: "{controller}/{action}/{first}_{last}",
                defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "gunName",
               url: "Guns/GunTypeAdd/{gName}",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "page",
                url: "{controller}/{action}/{page}",
                defaults: new { controller = "Home", action = "Index", page = UrlParameter.Optional });

            
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/",
                defaults: new { controller = "Home", action = "Index"});

            


        }
    }
}

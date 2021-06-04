using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IAProject_FreelancerSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Wall", action = "Index"}
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{action}",
                defaults: new { controller = "Dashboard", action = "Profile" }
            );

            routes.MapRoute(
                name: "Wall",
                url: "Wall/{action}",
                defaults: new { controller = "Wall", action = "Index" }
            );
            routes.MapRoute(
                 name: "FactoryLayout",
                 url: "FactoryLayout/{action}",
                 defaults: new { controller = "FactoryLayout", action = "Profile" }
            );

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppsGenerator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "MyApps",
                url: "MyApps",
                defaults: new { controller = "Application", action = "Index" }
            );

            routes.MapRoute(
                name: "ConfirmSignup",
                url: "ConfirmSignup",
                defaults: new { controller = "Member", action = "ConfirmSignup" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );
            
        }
    }
}

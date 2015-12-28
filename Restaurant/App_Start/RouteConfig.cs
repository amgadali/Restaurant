using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Restaurant
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // UserAddresses/Index/uName
            routes.MapRoute(
                name: "UserAddressesIdex",
                url: "UserAddresses/Index/{uName}",
                defaults: new
                {
                    controller = "UserAddresses",
                    action = "Index",
                }
            );
            // UserAddresses/Index/uName
            routes.MapRoute(
                name: "UserAddressesOther",
                url: "UserAddresses/{Action}/{id}",
                defaults: new
                {
                    controller = "UserAddresses",
                    action = "Details",
                }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}"); 
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "Restaurant.Controllers" }
          ).DataTokens["UseNamespaceFallback"] = false;

            
            

          
        }
    }
}

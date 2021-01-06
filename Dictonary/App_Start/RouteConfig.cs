using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewsPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional,sec=UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Category",
            url: "{controller}/{action}/{categoryId}/{category}",
            defaults: new { controller = "Home",action = "Category", categoryId = UrlParameter.Optional, category = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "News",
            url: "{action}/{Category}/{Year}/{Month}/{Id}/{SlugUrl}",
            defaults: new { controller = "Home",action = "News", Category= UrlParameter.Optional, Year = UrlParameter.Optional, Month = UrlParameter.Optional, Id = UrlParameter.Optional,  SlugUrl = UrlParameter.Optional }
            );

           routes.MapRoute(
           name: "Tags",
           url: "{action}/{search}",
           defaults: new { controller = "Home", action = "Topic",  search = UrlParameter.Optional }
           );
        }
    }
}

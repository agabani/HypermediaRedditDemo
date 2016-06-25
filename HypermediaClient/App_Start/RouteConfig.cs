using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HypermediaClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Reddit",
                url: "r/{subRedditName}/{action}/{commentId}/{commentName}",
                defaults: new
                {
                    subRedditName = UrlParameter.Optional,
                    commentId = UrlParameter.Optional,
                    commentName = UrlParameter.Optional,
                    controller = "Reddit",
                    action = "Comments"
                });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Reddit", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

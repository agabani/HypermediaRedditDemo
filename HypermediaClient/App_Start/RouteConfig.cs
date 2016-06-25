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
                "Proxy",
                "{*url}",
                new {controller = "Home", action = "Index", url = UrlParameter.Optional}
                );

            routes.MapRoute(
                "Reddit",
                "r/{subRedditName}/{action}/{commentId}/{commentName}",
                new
                {
                    subRedditName = UrlParameter.Optional,
                    commentId = UrlParameter.Optional,
                    commentName = UrlParameter.Optional,
                    controller = "Reddit",
                    action = "Comments"
                });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Reddit", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}
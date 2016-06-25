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
                "Reddit",
                "{*url}",
                new {controller = "Home", action = "Index", url = UrlParameter.Optional}
                );
        }
    }
}
using System.Web.Mvc;
using System.Web.Routing;

namespace HypermediaClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Ignore("default");
            routes.Ignore("nsfw");
            routes.Ignore("self");

            routes.MapRoute(
                "Reddit",
                "{*url}",
                new {controller = "Home", action = "Index", url = UrlParameter.Optional}
                );
        }
    }
}
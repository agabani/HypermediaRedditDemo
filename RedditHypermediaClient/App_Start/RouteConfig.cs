using System.Web.Mvc;
using System.Web.Routing;

namespace RedditHypermediaClient
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
                "RedditHypermediaClient",
                "{*url}",
                new
                {
                    controller = "RedditHypermediaClient",
                    action = "Index",
                    url = UrlParameter.Optional
                });
        }
    }
}
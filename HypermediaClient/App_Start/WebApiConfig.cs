using System.Web.Http;
using FluentSiren.AspNet.WebApi.Formatting;

namespace HypermediaClient
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Add(new SirenMediaFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "RedditApi",
                routeTemplate: "api/{*url}",
                defaults: new
                {
                    controller = "Reddit"
                }
            );
        }
    }
}

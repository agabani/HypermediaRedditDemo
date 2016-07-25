using System.Web.Http;
using FluentSiren.AspNet.WebApi.Formatting;

namespace HypermediaClient
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Add(new SirenMediaFormatter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "RedditApi",
                "api/{*url}",
                new
                {
                    controller = "Reddit"
                });
        }
    }
}
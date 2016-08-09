using System.Web.Http;
using System.Web.Http.Cors;
using FluentSiren.AspNet.WebApi.Formatting;

namespace RedditHypermediaApi
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            configuration.Formatters.Add(new SirenMediaFormatter());

            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                "RedditHypermediaApi",
                "{*url}",
                new
                {
                    controller = "RedditHypermediaApi"
                });
        }
    }
}
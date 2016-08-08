using System.Web.Http;
using FluentSiren.AspNet.WebApi.Formatting;

namespace RedditHypermediaApi
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
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
using System.Web.Mvc;
using HypermediaClient.Extensions;
using HypermediaClient.Hypermedia.Siren;

namespace HypermediaClient.Controllers
{
    [RoutePrefix("r")]
    public class RedditController : Controller
    {
        public ActionResult Index()
        {
            return View(new SirenClient(Request.BaseAddress()).Get("/api/root"));
        }

        [Route("{subRedditName}/comments/{commentId}/{commentName}")]
        public ActionResult Comments(string subRedditName, string commentId, string commentName)
        {
            return View(new SirenClient(Request.BaseAddress()).Get($"/api/RedditApi/{subRedditName}/comments/{commentId}/{commentName}"));
        }
    }
}
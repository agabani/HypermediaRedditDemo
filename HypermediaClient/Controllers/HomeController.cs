using System.Web.Mvc;
using HypermediaClient.Extensions;
using HypermediaClient.Hypermedia.Siren;

namespace HypermediaClient.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string url)
        {
            var api = url == null ? "/api" : $"/api/{url}";

            return View(new SirenClient(Request.BaseAddress()).Get(api));
        }
    }
}
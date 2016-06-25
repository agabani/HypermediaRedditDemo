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
            return View(new SirenClient(Request.BaseAddress()).Get(url == null ? "/api" : $"/api/{url}"));
        }
    }
}
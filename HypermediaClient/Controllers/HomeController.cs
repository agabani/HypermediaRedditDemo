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
            var relativeUri = url == null ? $"/api{Request.Query()}" : $"/api/{url}{Request.Query()}";
            return View(new SirenClient(Request.BaseAddress()).Get(relativeUri));
        }
    }
}
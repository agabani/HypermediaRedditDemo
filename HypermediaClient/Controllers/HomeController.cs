using System;
using System.Web.Mvc;
using HypermediaClient.Hypermedia.Siren;

namespace HypermediaClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string url = "/api/root")
        {
            ViewBag.Title = "Home Page";

            var sirenClient = new SirenClient(new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.Trim('/') + "/"));

            var entity = Proxy.PrependHref("/?url=",sirenClient.Get(url));

            return View(entity);
        }
    }
}
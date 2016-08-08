using System;
using System.Configuration;
using System.Web.Mvc;
using RedditHypermediaClient.Extensions;
using RedditHypermediaClient.Hypermedia.Siren;

namespace RedditHypermediaClient.Controllers
{
    public class RedditHypermediaClientController : Controller
    {
        // GET: RedditHypermediaClient
        public ActionResult Index(string url)
        {
            var relativeUri = url == null ? $"{Request.Query()}" : $"/{url}{Request.Query()}";
            return View(new SirenClient(new Uri(ConfigurationManager.AppSettings["hypermedia.api.baseurl"])).Get(relativeUri));
        }
    }
}
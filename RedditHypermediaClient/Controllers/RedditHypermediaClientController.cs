using System;
using System.Configuration;
using System.Web.Mvc;
using RedditHypermediaClient.Clients.Siren;
using RedditHypermediaClient.Extensions;

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
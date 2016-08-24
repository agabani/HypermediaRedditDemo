using System;
using System.Configuration;
using System.Web.Mvc;
using RedditHypermediaClient.Clients.Siren;
using RedditHypermediaClient.Extensions;
using RedditHypermediaClient.Services;

namespace RedditHypermediaClient.Controllers
{
    public class RedditHypermediaClientController : Controller
    {
        // GET: RedditHypermediaClient
        public ActionResult Index(string url)
        {
            var relativeUri = url == null ? $"{Request.Query()}" : $"/{url}{Request.Query()}";

            var entity = new SirenClient(new Uri(ConfigurationManager.AppSettings["hypermedia.api.baseurl"])).Get(relativeUri);
            //return View(entity);

            var preRender = new PreRenderService().PreRender(entity);

            return View(preRender);
        }
    }
}
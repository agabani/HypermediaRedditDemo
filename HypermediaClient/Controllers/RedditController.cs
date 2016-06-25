using System.Web.Http;
using HypermediaClient.Services;

namespace HypermediaClient.Controllers
{
    public class RedditController : ApiController
    {
        public IHttpActionResult Get(string url)
        {
            var relativeUri = url + Request.RequestUri.Query;
            return Ok(new RedditHypermediaService().Get(relativeUri));
        }
    }
}
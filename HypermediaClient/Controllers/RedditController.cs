using System.Web.Http;
using HypermediaClient.Services;

namespace HypermediaClient.Controllers
{
    public class RedditController : ApiController
    {
        public IHttpActionResult Get(string url)
        {
            return Ok(new RedditHypermediaService().Get(url));
        }
    }
}
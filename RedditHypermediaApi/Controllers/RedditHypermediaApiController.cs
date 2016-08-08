using System.Web.Http;
using RedditHypermediaApi.Services;

namespace RedditHypermediaApi.Controllers
{
    public class RedditHypermediaApiController : ApiController
    {
        public IHttpActionResult Get(string url)
        {
            var relativeUri = url + Request.RequestUri.Query;
            return Ok(new RedditHypermediaService().Get(relativeUri));
        }
    }
}

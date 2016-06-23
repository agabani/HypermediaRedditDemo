using System.Web.Http;
using FluentSiren.Builders;

namespace HypermediaClient.Controllers
{
    public class RootController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            var entity = new EntityBuilder()
                .WithClass("root")
                .WithClass("subreddit")
                .WithClass("pagination")
                .WithLink(new LinkBuilder()
                    .WithClass("listing")
                    .WithRel("listing")
                    .WithHref("/api/root/hot")
                    .WithTitle("hot"))
                .WithLink(new LinkBuilder()
                    .WithClass("listing")
                    .WithRel("listing")
                    .WithHref("/api/root/new")
                    .WithTitle("new"))
                .WithLink(new LinkBuilder()
                    .WithClass("listing")
                    .WithRel("listing")
                    .WithHref("/api/root/rising")
                    .WithTitle("rising"))
                .Build();

            return Ok(entity);
        }

        // GET api/values/5e
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
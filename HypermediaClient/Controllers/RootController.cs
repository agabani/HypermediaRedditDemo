using System;
using System.Linq;
using System.Web.Http;
using FluentSiren.Builders;
using RedditSharp;

namespace HypermediaClient.Controllers
{
    public class RootController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            var entityBuilder = new EntityBuilder()
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
                .WithLink(new LinkBuilder()
                    .WithClass("pagination")
                    .WithRel("next")
                    .WithHref("/api/root?page=2")
                    .WithTitle("next"));

            foreach (var post in new Reddit().FrontPage.Posts.Take(25))
            {
                var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                    .WithClass("post")
                    .WithRel("post")
                    .WithTitle(post.Title)
                    .WithProperty("score", post.Score)
                    .WithProperty("subreddit", post.SubredditName)
                    .WithProperty("comments", post.CommentCount)
                    .WithProperty("submitted", post.CreatedUTC)
                    .WithProperty("authorName", post.AuthorName)
                    .WithProperty("domain", post.Domain)
                    .WithProperty("linkFlairText", post.LinkFlairText)
                    .WithLink(new LinkBuilder()
                        .WithRel("self")
                        .WithHref(post.Permalink.ToString()));

                if (post.Thumbnail.OriginalString != string.Empty)
                {
                    embeddedRepresentationBuilder
                        .WithProperty("thumbnail", post.Thumbnail)
                        .WithProperty("url", post.Url);
                }

                entityBuilder.WithSubEntity(embeddedRepresentationBuilder);
            }

            return Ok(entityBuilder.Build());
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
using System;
using System.Linq;
using System.Web.Http;
using FluentSiren.Builders;
using RedditSharp;
using RedditSharp.Things;

namespace HypermediaClient.Controllers
{
    [RoutePrefix("r")]
    public class RedditController : ApiController
    {
        [Route("{subRedditName}/comments/{commentId}/{commentName}")]
        public IHttpActionResult Get(string subRedditName, string commentId, string commentName)
        {
            var post = new Reddit().GetPost(new Uri($"https://www.reddit.com/r/{subRedditName}/comments/{commentId}/{commentName}"));

            var entityBuilder = new EntityBuilder()
                .WithClass("post")
                .WithTitle(post.Title);

            BuildCommentTree(post, entityBuilder, 3);

            return Ok(entityBuilder.Build());
        }

        private static void BuildCommentTree(Post post, EntityBuilder builder, int depth)
        {
            if (depth > 0)
            {
                foreach (var comment in post.Comments.Take(10))
                {
                    var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                        .WithRel("comment")
                        .WithTitle(comment.Body);

                    BuildCommentTree(comment, embeddedRepresentationBuilder, depth--);

                    builder.WithSubEntity(embeddedRepresentationBuilder);
                }
            }
        }

        private static void BuildCommentTree(Comment post, EmbeddedRepresentationBuilder builder, int depth)
        {
            if (depth > 0)
            {
                foreach (var comment in post.Comments.Take(10))
                {
                    var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                        .WithRel("comment")
                        .WithTitle(comment.Body);

                    BuildCommentTree(comment, embeddedRepresentationBuilder, depth--);

                    builder.WithSubEntity(embeddedRepresentationBuilder);
                }
            }
        }
    }
}
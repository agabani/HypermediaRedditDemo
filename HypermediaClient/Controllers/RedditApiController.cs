﻿using System;
using System.Linq;
using System.Web.Http;
using FluentSiren.Builders;
using RedditSharp;
using RedditSharp.Things;

namespace HypermediaClient.Controllers
{
    [RoutePrefix("api/RedditApi")]
    public class RedditApiController : ApiController
    {
        [Route("{subRedditName}/comments/{commentId}/{commentName}")]
        public IHttpActionResult Get(string subRedditName, string commentId, string commentName)
        {
            var post = new Reddit().GetPost(new Uri($"https://www.reddit.com/r/{subRedditName}/comments/{commentId}/{commentName}"));

            var entityBuilder = new EntityBuilder()
                .WithClass("post")
                .WithProperty("linkFlairText", post.LinkFlairText)
                .WithTitle(post.Title)
                .WithProperty("score", post.Score)
                .WithProperty("domain", post.Domain)
                .WithProperty("subreddit", post.SubredditName)
                .WithProperty("submitted", post.CreatedUTC)
                .WithProperty("authorName", post.AuthorName);

            if (post.Thumbnail.OriginalString != string.Empty)
            {
                entityBuilder
                    .WithProperty("thumbnail", post.Thumbnail)
                    .WithProperty("url", post.Url);
            }

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
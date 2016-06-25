using System.Collections.Generic;
using FluentSiren.Builders;
using FluentSiren.Models;
using RedditSharp.Things;

namespace HypermediaClient.Services.Builders
{
    public class SearchBuilder
    {
        public SearchBuilder WithQuery(string query)
        {
            return this;
        }

        public Entity Build(IEnumerable<Thing> things)
        {
            var entityBuilder = new EntityBuilder()
                .WithClass("search")
                .WithLink(new LinkBuilder()
                    .WithClass("pagination")
                    .WithRel("next")
                    .WithHref("/api/root?page=2")
                    .WithTitle("next"));

            foreach (var thing in things)
            {
                var subreddit = thing as Subreddit;
                if (subreddit != null)
                {
                    entityBuilder
                        .WithSubEntity(Build(subreddit));
                }

                var post = thing as Post;
                if (post != null)
                {
                    entityBuilder
                        .WithSubEntity(Build(post));
                }
            }

            return entityBuilder
                .Build();
        }

        private static EmbeddedRepresentationBuilder Build(Subreddit subreddit)
        {
            var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                .WithClass("subreddit")
                .WithRel("subreddit")
                .WithTitle(subreddit.Title)
                .WithProperty("domain", subreddit.Url.OriginalString)
                .WithProperty("subscribers", subreddit.Subscribers)
                .WithProperty("created", subreddit.Created)
                .WithProperty("description", subreddit.PublicDescription);

            return embeddedRepresentationBuilder;
        }

        private static EmbeddedRepresentationBuilder Build(Post post)
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
                        .WithProperty("url", post.Url);

                if (post.Thumbnail.OriginalString == "self" || post.Thumbnail.OriginalString == "nsfw" || post.Thumbnail.OriginalString == "default")
                {
                    embeddedRepresentationBuilder
                        .WithProperty("thumbnail", "/" + post.Thumbnail);
                }
                else
                {
                    embeddedRepresentationBuilder
                        .WithProperty("thumbnail", post.Thumbnail);
                }
            }

            return embeddedRepresentationBuilder;
        }
    }
}
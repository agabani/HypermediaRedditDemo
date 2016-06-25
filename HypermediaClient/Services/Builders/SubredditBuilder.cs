using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using RedditSharp.Things;

namespace HypermediaClient.Services.Builders
{
    public class SubredditBuilder
    {
        public Entity Build(Subreddit subreddit)
        {
            var entityBuilder = new EntityBuilder()
                .WithClass("root")
                .WithClass("subreddit")
                .WithClass("pagination")
                //.WithLink(new LinkBuilder()
                //    .WithClass("listing")
                //    .WithRel("listing")
                //    .WithHref("/api/root/hot")
                //    .WithTitle("hot"))
                //.WithLink(new LinkBuilder()
                //    .WithClass("listing")
                //    .WithRel("listing")
                //    .WithHref("/api/root/new")
                //    .WithTitle("new"))
                //.WithLink(new LinkBuilder()
                //    .WithClass("listing")
                //    .WithRel("listing")
                //    .WithHref("/api/root/rising")
                //    .WithTitle("rising"))
                //.WithLink(new LinkBuilder()
                //    .WithClass("pagination")
                //    .WithRel("next")
                //    .WithHref("/api/root?page=2")
                //    .WithTitle("next"))
                .WithAction(new ActionBuilder()
                    .WithName("search")
                    .WithTitle("Search")
                    .WithMethod("GET")
                    .WithHref("/search")
                    .WithField(new FieldBuilder()
                        .WithName("q")
                        .WithType("text")));

            foreach (var post in subreddit.Posts.Take(25))
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

                entityBuilder.WithSubEntity(embeddedRepresentationBuilder);
            }

            return entityBuilder.Build();
        }
    }
}
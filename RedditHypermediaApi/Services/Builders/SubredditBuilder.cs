using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using RedditSharp;
using RedditSharp.Things;

namespace RedditHypermediaApi.Services.Builders
{
    public class SubredditBuilder
    {
        private int? _count;
        private string _order;

        public SubredditBuilder WithCount(int count)
        {
            _count = count;
            return this;
        }

        public SubredditBuilder WhereOrder(string order)
        {
            _order = order;
            return this;
        }

        public Entity Build(Subreddit subreddit)
        {
            var previousPage = _count == null || _count <= 0 ? null : _count < 25 ? 0 : _count - 25;
            var nextPage = (_count ?? 0) + 25;

            var entityBuilder = new EntityBuilder()
                .WithClass("root")
                .WithClass("subreddit")
                .WithClass("pagination")
                .WithLink(new LinkBuilder()
                    .WithClass("pagination")
                    .WithRel("next")
                    .WithHref($"?count={nextPage}")
                    .WithTitle("Next"))
                .WithAction(new ActionBuilder()
                    .WithName("search")
                    .WithTitle("Search")
                    .WithMethod("GET")
                    .WithHref("/search")
                    .WithField(new FieldBuilder()
                        .WithName("q")
                        .WithType("text")));

            BuildListings(entityBuilder, subreddit);

            if (previousPage != null)
            {
                entityBuilder
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("previous")
                        .WithHref($"?count={previousPage}")
                        .WithTitle("Previous"));
            }

            Listing<Post> listing;

            switch (_order)
            {
                case "hot":
                    listing = subreddit.Hot;
                    break;
                case "new":
                    listing = subreddit.New;
                    break;
                case "rising":
                    listing = subreddit.Rising;
                    break;
                case "top":
                    listing = subreddit.GetTop(FromTime.Day);
                    break;
                default:
                    listing = subreddit.Posts;
                    break;
            }

            foreach (var post in listing.Skip(_count ?? 0).Take(25))
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

        private void BuildListings(EntityBuilder entityBuilder, Subreddit subreddit)
        {
            BuildListing(entityBuilder, subreddit, "hot");
            BuildListing(entityBuilder, subreddit, "new");
            BuildListing(entityBuilder, subreddit, "rising");
            BuildListing(entityBuilder, subreddit, "top");
        }

        private void BuildListing(EntityBuilder entityBuilder, Subreddit subreddit, string listing)
        {
            var linkBuilder = new LinkBuilder()
                .WithClass("listing")
                .WithRel("listing")
                .WithHref($"{subreddit.Url.OriginalString}{listing}")
                .WithTitle(listing);

            if (listing == _order)
            {
                linkBuilder
                    .WithClass("active");
            }

            entityBuilder
                .WithLink(linkBuilder);
        }
    }
}
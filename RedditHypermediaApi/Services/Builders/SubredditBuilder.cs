using System.Collections.Generic;
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
        private string _since;

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

        public SubredditBuilder Since(string fromTime)
        {
            _since = fromTime;
            return this;
        }

        public Entity Build(Subreddit subreddit)
        {
            var entityBuilder = new EntityBuilder()
                .WithClass("root")
                .WithClass("subreddit");

            BuildBrand(entityBuilder);
            BuildListings(entityBuilder, subreddit);
            BuildSearch(entityBuilder);
            BuildPagination(entityBuilder);
            BuildPosts(subreddit, entityBuilder);

            return entityBuilder.Build();
        }

        private static void BuildBrand(EntityBuilder entityBuilder)
        {
            entityBuilder
                .WithLink(new LinkBuilder()
                    .WithTitle("Hypermedia Reddit")
                    .WithClass("brand")
                    .WithClass("navigation")
                    .WithRel("root")
                    .WithRel("navigation")
                    .WithHref("/"));
        }

        private void BuildListings(EntityBuilder entityBuilder, Subreddit subreddit)
        {
            BuildListingOption(entityBuilder, subreddit, "hot");
            BuildListingOption(entityBuilder, subreddit, "new");
            BuildListingOption(entityBuilder, subreddit, "rising");
            BuildListingOption(entityBuilder, subreddit, "top");

            BuildListingDuration(entityBuilder, subreddit, "top", "hour", "past hour");
            BuildListingDuration(entityBuilder, subreddit, "top", "day", "past day");
            BuildListingDuration(entityBuilder, subreddit, "top", "week", "past week");
            BuildListingDuration(entityBuilder, subreddit, "top", "month", "past month");
            BuildListingDuration(entityBuilder, subreddit, "top", "year", "past year");
            BuildListingDuration(entityBuilder, subreddit, "top", "all", "all time");
        }

        private void BuildListingOption(EntityBuilder entityBuilder, Subreddit subreddit, string listing)
        {
            var linkBuilder = new LinkBuilder()
                .WithClass("navigation")
                .WithRel("listing")
                .WithRel(listing)
                .WithHref($"{subreddit.Url.OriginalString}{listing}")
                .WithTitle(listing);

            if (listing == _order && _since == null)
            {
                linkBuilder
                    .WithClass("active");
            }

            entityBuilder
                .WithLink(linkBuilder);
        }

        private void BuildListingDuration(EntityBuilder entityBuilder, Subreddit subreddit, string listing, string duration, string durataionText)
        {
            var linkBuilder = new LinkBuilder()
                .WithClass("navigation")
                .WithRel("listing.time")
                .WithRel(listing)
                .WithHref($"{subreddit.Url.OriginalString}{listing}/?sort={listing}&t={duration}")
                .WithTitle(durataionText);

            if (duration == _since)
            {
                linkBuilder
                    .WithClass("active");
            }

            entityBuilder
                .WithLink(linkBuilder);
        }

        private void BuildPagination(EntityBuilder entityBuilder)
        {
            var previousPage = _count == null || _count <= 0 ? null : _count < 25 ? 0 : _count - 25;
            var nextPage = (_count ?? 0) + 25;

            entityBuilder
                .WithClass("pagination")
                .WithLink(new LinkBuilder()
                    .WithClass("pagination")
                    .WithRel("next")
                    .WithHref($"?count={nextPage}")
                    .WithTitle("Next"));

            if (previousPage != null)
            {
                entityBuilder
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("previous")
                        .WithHref($"?count={previousPage}")
                        .WithTitle("Previous"));
            }
        }

        private void BuildSearch(EntityBuilder entityBuilder)
        {
            entityBuilder
                .WithAction(new ActionBuilder()
                    .WithClass("navigation")
                    .WithName("search")
                    .WithTitle("Search")
                    .WithMethod("GET")
                    .WithHref("/search")
                    .WithField(new FieldBuilder()
                        .WithName("q")
                        .WithType("text")));
        }

        private void BuildPosts(Subreddit subreddit, EntityBuilder entityBuilder)
        {
            var listing = GetPosts(subreddit);

            foreach (var post in listing.Skip(_count ?? 0).Take(25))
            {
                BuildPost(entityBuilder, post);
            }
        }

        private IEnumerable<Post> GetPosts(Subreddit subreddit)
        {
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
                    switch (_since)
                    {
                        case "hour":
                            listing = subreddit.GetTop(FromTime.Hour);
                            break;
                        case "day":
                            listing = subreddit.GetTop(FromTime.Day);
                            break;
                        case "week":
                            listing = subreddit.GetTop(FromTime.Week);
                            break;
                        case "month":
                            listing = subreddit.GetTop(FromTime.Month);
                            break;
                        case "year":
                            listing = subreddit.GetTop(FromTime.Year);
                            break;
                        case "all":
                            listing = subreddit.GetTop(FromTime.All);
                            break;
                        default:
                            listing = subreddit.GetTop(FromTime.Week);
                            break;
                    }
                    break;
                default:
                    listing = subreddit.Posts;
                    break;
            }
            return listing;
        }

        private static void BuildPost(EntityBuilder entityBuilder, Post post)
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
    }
}
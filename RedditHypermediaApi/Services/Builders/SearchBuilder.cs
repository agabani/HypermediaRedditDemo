using System.Collections.Generic;
using FluentSiren.Builders;
using FluentSiren.Models;
using RedditSharp.Things;

namespace RedditHypermediaApi.Services.Builders
{
    public class SearchBuilder
    {
        private int? _count;

        private bool _onlySubreddit, _onlyPost;
        private string _query;

        public SearchBuilder WithQuery(string query)
        {
            _query = query;
            return this;
        }

        public SearchBuilder OnlySubreddit()
        {
            _onlySubreddit = true;
            _onlyPost = false;
            return this;
        }

        public SearchBuilder OnlyPost()
        {
            _onlyPost = true;
            _onlySubreddit = false;
            return this;
        }

        public SearchBuilder WithCount(int count)
        {
            _count = count;
            return this;
        }

        public Entity Build(IEnumerable<Thing> things)
        {
            var previousPage = _count == null || _count <= 0 ? null : _count < 25 ? 0 : _count - 25;
            var nextPage = _count + 25;

            var entityBuilder = new EntityBuilder()
                .WithClass("search");

            BuildBrand(entityBuilder);
            BuildSearch(entityBuilder);

            if (_onlySubreddit)
            {
                entityBuilder
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("next")
                        .WithRel("subreddit")
                        .WithHref($"?q={_query}&type=subreddit&count={nextPage}")
                        .WithTitle("next"));

                if (previousPage != null)
                {
                    entityBuilder
                        .WithLink(new LinkBuilder()
                            .WithClass("pagination")
                            .WithRel("previous")
                            .WithRel("subreddit")
                            .WithHref($"?q={_query}&type=subreddit&count={previousPage}")
                            .WithTitle("previous"));
                }
            }
            else if (_onlyPost)
            {
                entityBuilder
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("next")
                        .WithRel("post")
                        .WithHref($"?q={_query}&type=post&count={nextPage}")
                        .WithTitle("next"));

                if (previousPage != null)
                {
                    entityBuilder
                        .WithLink(new LinkBuilder()
                            .WithClass("pagination")
                            .WithRel("previous")
                            .WithRel("post")
                            .WithHref($"?q={_query}&type=post&count={previousPage}")
                            .WithTitle("previous"));
                }
            }
            else
            {
                entityBuilder
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("next")
                        .WithRel("subreddit")
                        .WithHref($"?q={_query}&type=subreddit&count=3")
                        .WithTitle("next"))
                    .WithLink(new LinkBuilder()
                        .WithClass("pagination")
                        .WithRel("next")
                        .WithRel("post")
                        .WithHref($"?q={_query}&type=post&count=22")
                        .WithTitle("next"));
            }

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
                        .WithType("text")
                        .WithValue(_query)));
        }

        private static EmbeddedRepresentationBuilder Build(Subreddit subreddit)
        {
            var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                .WithClass("subreddit")
                .WithRel("subreddit")
                .WithTitle(subreddit.Title)
                .WithProperty("domain", $"/r/{subreddit.DisplayName}")
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
                    .WithHref(post.Url.LocalPath));

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
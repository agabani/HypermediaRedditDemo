using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using RedditSharp.Things;

namespace HypermediaClient.Services.Builders
{
    public class PostBuilder
    {
        public Entity Build(Post post)
        {
            var entityBuilder = new EntityBuilder()
                .WithClass("post")
                .WithProperty("linkFlairText", post.LinkFlairText)
                .WithTitle(post.Title)
                .WithProperty("score", post.Score)
                .WithProperty("domain", post.Domain)
                .WithProperty("subreddit", post.SubredditName)
                .WithProperty("submitted", post.CreatedUTC)
                .WithProperty("authorName", post.AuthorName)
                .WithProperty("comments", post.CommentCount)
                .WithProperty("selfText", post.SelfText);

            if (post.Thumbnail.OriginalString != string.Empty)
            {
                entityBuilder
                    .WithProperty("url", post.Url);

                if (post.Thumbnail.OriginalString == "self" || post.Thumbnail.OriginalString == "nsfw" || post.Thumbnail.OriginalString == "default")
                {
                    entityBuilder
                        .WithProperty("thumbnail", "/" + post.Thumbnail);
                }
                else
                {
                    entityBuilder
                        .WithProperty("thumbnail", post.Thumbnail);
                }
            }

            BuildCommentTree(post, entityBuilder);

            return entityBuilder.Build();
        }

        private static void BuildCommentTree(Post post, EntityBuilder builder)
        {
            foreach (var comment in post.Comments.Where(c => c.Kind != "more"))
            {
                var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                    .WithRel("comment")
                    .WithProperty("author", comment.Author)
                    .WithProperty("score", comment.Score)
                    .WithProperty("created", comment.CreatedUTC)
                    .WithTitle(comment.Body);

                BuildCommentTree(comment, embeddedRepresentationBuilder);

                builder.WithSubEntity(embeddedRepresentationBuilder);
            }
        }

        private static void BuildCommentTree(Comment post, EmbeddedRepresentationBuilder builder)
        {
            foreach (var comment in post.Comments.Where(c => c.Kind != "more"))
            {
                var embeddedRepresentationBuilder = new EmbeddedRepresentationBuilder()
                    .WithRel("comment")
                    .WithProperty("author", comment.Author)
                    .WithProperty("score", comment.Score)
                    .WithProperty("created", comment.CreatedUTC)
                    .WithTitle(comment.Body);

                BuildCommentTree(comment, embeddedRepresentationBuilder);

                builder.WithSubEntity(embeddedRepresentationBuilder);
            }
        }
    }
}
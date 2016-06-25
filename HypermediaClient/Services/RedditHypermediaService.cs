using System;
using System.Text.RegularExpressions;
using FluentSiren.Models;
using HypermediaClient.Services.Builders;
using RedditSharp;

namespace HypermediaClient.Services
{
    public class RedditHypermediaService
    {
        private static readonly Uri RedditBaseAddress = new Uri("https://www.reddit.com");
        private static readonly Regex PostUrlPattern = new Regex(@"r\/(\w+)\/comments\/(\w+)\/(\w*)");

        public Entity Get(string url)
        {
            if (url == null)
            {
                return FrontPage();
            }

            if (IsPost(url))
            {
                return Post(url);
            }

            return FrontPage();
        }

        private static bool IsPost(string url)
        {
            return PostUrlPattern.IsMatch(url);
        }

        private static Entity FrontPage()
        {
            return new SubredditBuilder().Build(new Reddit().FrontPage);
        }

        private static Entity Post(string url)
        {
            return new PostBuilder().Build(new Reddit().GetPost(new Uri(RedditBaseAddress, url)));
        }
    }
}
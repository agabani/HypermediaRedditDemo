using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentSiren.Models;
using HypermediaClient.Services.Builders;
using RedditSharp;
using RedditSharp.Things;

namespace HypermediaClient.Services
{
    public class RedditHypermediaService
    {
        private static readonly Uri RedditBaseAddress = new Uri("https://www.reddit.com");
        private static readonly Regex PostUrlPattern = new Regex(@"r\/(\w+)\/comments\/(\w+)\/(\w*)");
        private static readonly Regex SearchUrlPattern = new Regex(@"search\?");

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

            if (IsSearch(url))
            {
                return Search(url);
            }

            return FrontPage();
        }

        private static bool IsPost(string url)
        {
            return PostUrlPattern.IsMatch(url);
        }

        private static bool IsSearch(string url)
        {
            return SearchUrlPattern.IsMatch(url);
        }

        private static Entity FrontPage()
        {
            return new SubredditBuilder().Build(new Reddit().FrontPage);
        }

        private static Entity Post(string url)
        {
            return new PostBuilder().Build(new Reddit().GetPost(new Uri(RedditBaseAddress, url)));
        }

        private static Entity Search(string url)
        {
            return new SearchBuilder().Build(new Reddit().Search<Thing>(url).Take(25));
        }
    }
}
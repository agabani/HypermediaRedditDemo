using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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
        private static readonly Regex SearchUrlPattern = new Regex(@"search\?.*(?:q=([\w\+\ ]+))");

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
            var query = SearchUrlPattern.Match(HttpUtility.UrlDecode(url)).Groups[1].Value;

            var things = new Reddit().SearchSubreddits(query).Take(3).Concat(new Reddit().Search<Thing>(query).Take(22));

            return new SearchBuilder().Build(things);
        }
    }
}
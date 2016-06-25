﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
        private static readonly Regex SubredditUrlPattern = new Regex(@"r\/([\w]+)");

        public Entity Get(string url)
        {
            if (url == null)
            {
                return FrontPage(url);
            }

            if (IsPost(url))
            {
                return Post(url);
            }

            if (IsSearch(url))
            {
                return Search(url);
            }

            if (IsSubreddit(url))
            {
                return SubReddit(url);
            }

            return FrontPage(url);
        }

        private static bool IsPost(string url)
        {
            return PostUrlPattern.IsMatch(url);
        }

        private static bool IsSearch(string url)
        {
            return SearchUrlPattern.IsMatch(url);
        }

        public static bool IsSubreddit(string url)
        {
            return SubredditUrlPattern.IsMatch(url);
        }

        private static Entity FrontPage(string url)
        {
            var subredditBuilder = new SubredditBuilder();

            if (url != null)
            {
                var nameValueCollection = HttpUtility.ParseQueryString(new Uri(RedditBaseAddress, url).Query);

                if (nameValueCollection.AllKeys.Contains("count"))
                {
                    subredditBuilder
                        .WithCount(int.Parse(nameValueCollection["count"]));
                }
            }

            return subredditBuilder.Build(new Reddit().FrontPage);
        }

        private static Entity Post(string url)
        {
            var uri = new Uri(RedditBaseAddress, url);
            return new PostBuilder().Build(new Reddit().GetPost(new Uri(RedditBaseAddress, uri.AbsolutePath)));
        }

        private static Entity Search(string url)
        {
            var query = SearchUrlPattern.Match(HttpUtility.UrlDecode(url)).Groups[1].Value;

            var things = new Reddit().SearchSubreddits(query).Take(3).Concat(new Reddit().Search<Thing>(query).Take(22));

            return new SearchBuilder().WithQuery(query).Build(things);
        }

        private static Entity SubReddit(string url)
        {
            var nameValueCollection = HttpUtility.ParseQueryString(new Uri(RedditBaseAddress, url).Query);

            var subredditBuilder = new SubredditBuilder();

            if (nameValueCollection.AllKeys.Contains("count"))
            {
                subredditBuilder
                    .WithCount(int.Parse(nameValueCollection["count"]));
            }

            var subRedditName = SubredditUrlPattern.Match(url).Groups[1].Value;
            return subredditBuilder.Build(new Reddit().GetSubreddit(subRedditName));
        }
    }
}
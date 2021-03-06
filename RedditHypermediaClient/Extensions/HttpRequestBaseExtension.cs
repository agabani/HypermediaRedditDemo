﻿using System;
using System.Linq;
using System.Web;

namespace RedditHypermediaClient.Extensions
{
    public static class HttpRequestBaseExtension
    {
        public static Uri BaseAddress(this HttpRequestBase request)
        {
            if (request.Url == null || request.ApplicationPath == null)
            {
                throw new NullReferenceException("Request url and application path cannot be null.");
            }

            return new Uri(request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.Trim('/') + "/");
        }

        public static string Query(this HttpRequestBase request)
        {
            var query = string.Join("&", request.QueryString.AllKeys.Select(q => $"{q}={request.QueryString[q]}"));
            return query == string.Empty ? null : $"?{query}";
        }
    }
}
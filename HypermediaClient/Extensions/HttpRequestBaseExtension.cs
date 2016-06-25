using System;
using System.Web;

namespace HypermediaClient.Extensions
{
    public static class HttpRequestBaseExtension
    {
        public static Uri BaseAddress(this HttpRequestBase request)
        {
            return new Uri(request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.Trim('/') + "/");
        }
    }
}
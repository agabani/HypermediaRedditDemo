﻿using System.Web.Optimization;

namespace RedditHypermediaClient
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/modernizr")
                    .Include("~/Scripts/modernizr-*"));

            bundles
                .Add(new ScriptBundle("~/bundles/bootstrap")
                    .Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles
                .Add(new StyleBundle("~/Content/css")
                    .Include(
                        "~/Content/bootstrap.css",
                        "~/Content/font-awesome.css",
                        "~/Content/site.css"));
        }
    }
}
﻿namespace Narvalo.Web
{
    using System;
    using System.Web;

    public static partial class HttpResponseExtensions
    {
        public static void NoCache(this HttpResponse @this)
        {
            Require.Object(@this);

            @this.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        public static void PubliclyCacheFor(this HttpResponse @this, int days, int hours, int minutes)
        {
            Require.Object(@this);

            @this.PubliclyCacheFor(new TimeSpan(days, hours, minutes, 0));
        }

        public static void PubliclyCacheFor(this HttpResponse @this, TimeSpan duration)
        {
            Require.Object(@this);

            @this.Cache.SetCacheability(HttpCacheability.Public);
            @this.CacheFor_(duration);
        }

        public static void PrivatelyCacheFor(this HttpResponse @this, int days, int hours, int minutes)
        {
            Require.Object(@this);

            @this.PrivatelyCacheFor(new TimeSpan(days, hours, minutes, 0));
        }

        public static void PrivatelyCacheFor(this HttpResponse @this, TimeSpan duration)
        {
            Require.Object(@this);

            // REVIEW: Utiliser HttpCacheability.ServerAndPrivate ?
            @this.Cache.SetCacheability(HttpCacheability.Private);
            @this.CacheFor_(duration);
        }

        public static void CacheFor(this HttpResponse @this, bool publicly, HttpVersions versions, TimeSpan duration)
        {
            Require.Object(@this);

            @this.Cache.SetCacheability(publicly ? HttpCacheability.Public : HttpCacheability.Private);

            // En-tête HTTP 1.0
            if ((versions & HttpVersions.HttpV10) == HttpVersions.HttpV10) {
                @this.Cache.SetExpires(DateTime.Now.Add(duration));
            }
            
            // En-tête HTTP 1.1
            if ((versions & HttpVersions.HttpV11) == HttpVersions.HttpV11) {
                @this.Cache.SetMaxAge(duration);
                @this.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }
        }

        static void CacheFor_(this HttpResponse @this, TimeSpan duration)
        {
            DebugCheck.NotNull(@this);

            // En-tête HTTP 1.0
            // REVIEW: Now ou UtcNow ?
            @this.Cache.SetExpires(DateTime.UtcNow.Add(duration));
            
            // En-tête HTTP 1.1
            @this.Cache.SetMaxAge(duration);
            @this.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            
            // REVIEW: Now ou UtcNow ?
            @this.Cache.SetLastModified(DateTime.Now);
        }
    }
}

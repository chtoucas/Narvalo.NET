// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Provides extension methods for <see cref="HttpResponse"/>.
    /// </summary>
    public static partial class HttpResponseExtensions
    {
        public static void SendPlainText(this HttpResponse @this, string content)
        {
            Require.NotNull(@this, nameof(@this));

            @this.ContentType = "text/plain";
            @this.Write(content);
        }

        public static void SetStatusCode(this HttpResponse @this, HttpStatusCode statusCode)
        {
            Require.NotNull(@this, nameof(@this));

            @this.StatusCode = (int)statusCode;
        }
    }

    public static partial class HttpResponseExtensions
    {
        public static void NoCache(this HttpResponse @this)
        {
            Require.NotNull(@this, nameof(@this));

            @this.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        public static void PubliclyCacheFor(this HttpResponse @this, int days, int hours, int minutes)
        {
            Expect.NotNull(@this);

            @this.PubliclyCacheFor(new TimeSpan(days, hours, minutes, 0));
        }

        public static void PubliclyCacheFor(this HttpResponse @this, TimeSpan duration)
        {
            Expect.NotNull(@this);

            @this.CacheFor(duration, HttpCacheability.Public);
        }

        public static void PrivatelyCacheFor(this HttpResponse @this, int days, int hours, int minutes)
        {
            Expect.NotNull(@this);

            @this.PrivatelyCacheFor(new TimeSpan(days, hours, minutes, 0));
        }

        public static void PrivatelyCacheFor(this HttpResponse @this, TimeSpan duration)
        {
            Expect.NotNull(@this);

            // REVIEW: Utiliser HttpCacheability.ServerAndPrivate ?
            @this.CacheFor(duration, HttpCacheability.Private);
        }

        public static void CacheFor(this HttpResponse @this, TimeSpan duration, HttpCacheability cacheability)
        {
            Expect.NotNull(@this);

            @this.CacheFor(duration, cacheability, HttpVersions.All);
        }

        public static void CacheFor(
            this HttpResponse @this,
            TimeSpan duration,
            HttpCacheability cacheability,
            HttpVersions versions)
        {
            Require.NotNull(@this, nameof(@this));

            @this.Cache.SetCacheability(cacheability);

            // En-tête HTTP 1.0
            if (versions.Contains(HttpVersions.HttpV10))
            {
                // REVIEW: Now ou UtcNow ?
                @this.Cache.SetExpires(DateTime.UtcNow.Add(duration));
            }

            // En-tête HTTP 1.1
            if (versions.Contains(HttpVersions.HttpV11))
            {
                @this.Cache.SetMaxAge(duration);
                @this.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }

            // REVIEW: Now ou UtcNow ?
            @this.Cache.SetLastModified(DateTime.Now);
        }
    }
}

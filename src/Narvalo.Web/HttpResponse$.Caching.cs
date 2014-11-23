namespace Narvalo.Web
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

            @this.CacheFor(duration, HttpCacheability.Public);
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
            @this.CacheFor(duration, HttpCacheability.Private);
        }

        public static void CacheFor(this HttpResponse @this, TimeSpan duration, HttpCacheability cacheability)
        {
            Check.NotNull(@this);

            @this.CacheFor(duration, cacheability, HttpVersions.All);
        }

        public static void CacheFor(this HttpResponse @this, TimeSpan duration, HttpCacheability cacheability, HttpVersions versions)
        {
            Require.Object(@this);

            @this.Cache.SetCacheability(cacheability);

            // En-tête HTTP 1.0
            if (versions.HasFlag(HttpVersions.HttpV10)) {
                // REVIEW: Now ou UtcNow ?
                @this.Cache.SetExpires(DateTime.UtcNow.Add(duration));
            }
            
            // En-tête HTTP 1.1
            if (versions.HasFlag(HttpVersions.HttpV11)) {
                @this.Cache.SetMaxAge(duration);
                @this.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }

            // REVIEW: Now ou UtcNow ?
            @this.Cache.SetLastModified(DateTime.Now);
        }
    }
}

// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Internal
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    internal static class PageExtensions
    {
        readonly static string CacheKey_ = typeof(PageExtensions).FullName;

        public static IEnumerable<object> FindHosts(this Page @this)
        {
            yield return @this;

            var masterHost = @this.Master;

            while (masterHost != null) {
                yield return masterHost;

                masterHost = masterHost.Master;
            }
        }

        public static PageHost GetOrAddHost(this Page @this, HttpContext context)
        {
            DebugCheck.NotNull(@this);
            DebugCheck.NotNull(context);

            var pageContext = @this.Items;

            if (pageContext.Contains(CacheKey_)) {
                return (PageHost)pageContext[CacheKey_];
            }
            else {
                var host = new PageHost(@this, context);
                pageContext[CacheKey_] = host;
                return host;
            }
        }
    }
}

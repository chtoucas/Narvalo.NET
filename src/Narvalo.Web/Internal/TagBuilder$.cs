// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Internal
{
    using System.Web;
    using System.Web.Mvc;
    using Narvalo;

    static class TagBuilderExtensions
    {
        public static IHtmlString ToHtmlString(this TagBuilder @this)
        {
            Check.NotNull(@this);

            return @this.ToHtmlString(TagRenderMode.Normal);
        }

        public static IHtmlString ToHtmlString(this TagBuilder @this, TagRenderMode renderMode)
        {
            Require.Object(@this);

            return new HtmlString(@this.ToString(renderMode));
        }
    }
}

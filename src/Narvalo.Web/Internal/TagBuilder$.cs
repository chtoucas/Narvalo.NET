// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Internal
{
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using Narvalo;

    internal static class TagBuilderExtensions
    {
        public static IHtmlString ToHtmlString(this TagBuilder @this)
        {
            Contract.Requires(@this != null);

            return @this.ToHtmlString(TagRenderMode.Normal);
        }

        public static IHtmlString ToHtmlString(this TagBuilder @this, TagRenderMode renderMode)
        {
            Require.Object(@this);

            return new HtmlString(@this.ToString(renderMode));
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Narvalo.Web.Internal;

    public static class LinkExtensions
    {
        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType, string relation)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            this HtmlHelper @this,
            Uri linkUri,
            string linkType,
            string relation,
            object attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(
            this HtmlHelper @this,
            Uri linkUri,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, linkType, relation, attributes);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        private static IHtmlString LinkHelper_(
            this HtmlHelper @this,
            Uri linkUri,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Require.NotNull(linkUri, "linkUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("link");
            tag.MergeAttribute("href", linkUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(linkType))
            {
                tag.MergeAttribute("type", linkType);
            }

            if (!String.IsNullOrEmpty(relation))
            {
                tag.MergeAttribute("rel", relation);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }
}

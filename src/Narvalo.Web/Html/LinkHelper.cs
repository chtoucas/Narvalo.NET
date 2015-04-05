// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Narvalo.Web.Internal;

    public static class LinkHelper
    {
        public static IHtmlString Render(Uri linkUri)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(linkUri, null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri linkUri, string linkType)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri linkUri, string linkType, string relation)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(
            Uri linkUri,
            string linkType,
            string relation,
            object attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(linkUri, linkType, relation, new RouteValueDictionary(attributes));
        }

        private static IHtmlString Render(
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

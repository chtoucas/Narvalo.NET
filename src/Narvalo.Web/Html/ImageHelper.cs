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

    public static class ImageHelper
    {
        public static IHtmlString Render(Uri imageUri)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(imageUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri imageUri, string alt)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(imageUri, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Render(Uri imageUri, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(imageUri, null, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Render(Uri imageUri, IDictionary<string, object> attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(imageUri, null, attributes);
        }

        public static IHtmlString Render(Uri imageUri, string alt, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Render(imageUri, alt, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Render(
            Uri imageUri,
            string alt,
            IDictionary<string, object> attributes)
        {
            Require.NotNull(imageUri, "imageUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", imageUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(alt))
            {
                tag.MergeAttribute("alt", alt);
            }

            tag.MergeAttributes(attributes, true);

            Contract.Assume(tag.Attributes != null);

            if (tag.Attributes.ContainsKey("alt") && !tag.Attributes.ContainsKey("title"))
            {
                tag.MergeAttribute("title", (tag.Attributes["alt"] ?? String.Empty).ToString());
            }

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }
}

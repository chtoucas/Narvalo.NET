// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Narvalo.Web.Internal;

    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri)
        {
            return Image(@this, imageUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt)
        {
            return Image(@this, imageUri, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, object attributes)
        {
            return Image(@this, imageUri, null, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, IDictionary<string, object> attributes)
        {
            return Image(@this, imageUri, null, attributes);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt, object attributes)
        {
            return Image(@this, imageUri, alt, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt, IDictionary<string, object> attributes)
        {
            return ImageHelper_(@this, imageUri, alt, ObjectToDictionary_(attributes));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this", Justification = "On utilise une méthode d'extension afin d'en améliorer son accessibilité.")]
        private static IHtmlString ImageHelper_(this HtmlHelper @this, Uri imageUri, string alt, IDictionary<string, object> attributes)
        {
            Require.NotNull(imageUri, "imageUri");

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", imageUri.ToProtocolLessString());

            if (!String.IsNullOrEmpty(alt))
            {
                tag.MergeAttribute("alt", alt);
            }

            tag.MergeAttributes(attributes, true);

            if (tag.Attributes.ContainsKey("alt") && !tag.Attributes.ContainsKey("title"))
            {
                tag.MergeAttribute("title", (tag.Attributes["alt"] ?? String.Empty).ToString());
            }

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }
}

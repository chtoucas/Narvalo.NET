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
            return Image(@this, imageUri, null, TypeUtility.ObjectToDictionary(attributes));
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, IDictionary<string, object> attributes)
        {
            return Image(@this, imageUri, null, attributes);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt, object attributes)
        {
            return Image(@this, imageUri, alt, TypeUtility.ObjectToDictionary(attributes));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt, IDictionary<string, object> attributes)
        {
            return ImageHelper_(imageUri, alt, TypeUtility.ObjectToDictionary(attributes));
        }

        static IHtmlString ImageHelper_(Uri imageUri, string alt, IDictionary<string, object> attributes)
        {
            Requires.NotNull(imageUri, "imageUri");

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", imageUri.ToProtocolLessString());

            if (!String.IsNullOrEmpty(alt)) {
                tag.MergeAttribute("alt", alt);
            }

            tag.MergeAttributes(attributes, true);

            if (tag.Attributes.ContainsKey("alt") && !tag.Attributes.ContainsKey("title")) {
                tag.MergeAttribute("title", (tag.Attributes["alt"] ?? String.Empty).ToString());
            }

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }
}

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Web.Internal;

    public partial class Tag
    {
        public IHtmlString Image(Uri imageUri)
        {
            return Image(imageUri, null, (IDictionary<string, object>)null);
        }

        public IHtmlString Image(Uri imageUri, string alt)
        {
            return Image(imageUri, alt, (IDictionary<string, object>)null);
        }

        public IHtmlString Image(Uri imageUri, object attributes)
        {
            return Image(imageUri, null, TypeHelpers.ObjectToDictionary(attributes));
        }

        public IHtmlString Image(Uri imageUri, IDictionary<string, object> attributes)
        {
            return Image(imageUri, null, attributes);
        }

        public IHtmlString Image(Uri imageUri, string alt, object attributes)
        {
            return Image(imageUri, alt, TypeHelpers.ObjectToDictionary(attributes));
        }

        public IHtmlString Image(Uri imageUri, string alt, IDictionary<string, object> attributes)
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

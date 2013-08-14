namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Web.Internal;

    public static partial class Tag
    {
        public static IHtmlString Link(Uri linkUri, string linkType)
        {
            return Link(linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(Uri linkUri, string linkType, string relation)
        {
            return Link(linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(Uri linkUri, string linkType, string relation, object attributes)
        {
            return Link(linkUri, linkType, relation, TypeHelpers.ObjectToDictionary(attributes));
        }

        public static IHtmlString Link(
            Uri linkUri,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Requires.NotNull(linkUri, "linkUri");

            var tag = new TagBuilder("link");
            tag.MergeAttribute("href", linkUri.ToProtocolLessString());

            if (!String.IsNullOrEmpty(linkType)) {
                tag.MergeAttribute("type", linkType);
            }
            if (!String.IsNullOrEmpty(relation)) {
                tag.MergeAttribute("rel", relation);
            }

            tag.MergeAttributes(attributes, true /* replaceExisting */);

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }
}

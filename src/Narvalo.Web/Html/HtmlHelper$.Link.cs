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
        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType)
        {
            return Link(@this, linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType, string relation)
        {
            return Link(@this, linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType, string relation, object attributes)
        {
            return Link(@this, linkUri, linkType, relation, TypeUtility.ObjectToDictionary(attributes));
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType, string relation, IDictionary<string, object> attributes)
        {
            return LinkHelper_(@this, linkUri, linkType, relation, attributes);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this", Justification = "On utilise une méthode d'extension afin d'améliorer son accessibilité.")]
        static IHtmlString LinkHelper_(this HtmlHelper @this, Uri linkUri, string linkType, string relation, IDictionary<string, object> attributes)
        {
            Require.NotNull(linkUri, "linkUri");

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

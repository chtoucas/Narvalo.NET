namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Narvalo.Web.Internal;

    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri)
        {
            return Script(@this, scriptUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri, string scriptType)
        {
            return Script(@this, scriptUri, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri, string scriptType, object attributes)
        {
            return Script(@this, scriptUri, scriptType, TypeUtility.ObjectToDictionary(attributes));
        }

        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri, string scriptType, IDictionary<string, object> attributes)
        {
            return ScriptHelper_(scriptUri, scriptType, attributes);
        }

        static IHtmlString ScriptHelper_(Uri scriptUri, string scriptType, IDictionary<string, object> attributes)
        {
            Requires.NotNull(scriptUri, "scriptUri");

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", scriptUri.ToProtocolLessString());

            if (!String.IsNullOrEmpty(scriptType)) {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, true);

            return tag.ToHtmlString();
        }
    }
}

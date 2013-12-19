namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Web.Internal;

    public partial class Tag
    {
        public IHtmlString Script(Uri scriptUri)
        {
            return Script(scriptUri, null, (IDictionary<string, object>)null);
        }

        public IHtmlString Script(Uri scriptUri, string scriptType)
        {
            return Script(scriptUri, scriptType, (IDictionary<string, object>)null);
        }

        public IHtmlString Script(Uri scriptUri, string scriptType, object attributes)
        {
            return Script(scriptUri, scriptType, TypeHelpers.ObjectToDictionary(attributes));
        }

        public IHtmlString Script(
            Uri scriptUri,
            string scriptType,
            IDictionary<string, object> attributes)
        {
            Requires.NotNull(scriptUri, "scriptUri");

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", scriptUri.ToProtocolLessString());

            if (!String.IsNullOrEmpty(scriptType)) {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, true);

            return tag.ToHtmlString(TagRenderMode.Normal);
        }
    }
}

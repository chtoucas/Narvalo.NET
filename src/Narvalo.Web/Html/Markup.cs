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

    public static class Markup
    {
        private static HtmlString s_Lipsum;

        public static IHtmlString LoremIpsum
        {
            get
            {
                Contract.Ensures(Contract.Result<IHtmlString>() != null);

                if (s_Lipsum == null)
                {
                    s_Lipsum = new HtmlString(
                        "Lorem ipsum dolor sit amet, consectetur adipisicing elit, "
                        + "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. "
                        + "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "
                        + "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in "
                        + "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. "
                        + "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia "
                        + "deserunt mollit anim id est laborum.");
                }

                return s_Lipsum;
            }
        }

        #region Image()

        public static IHtmlString Image(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, string alt)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, object attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), null, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, IDictionary<string, object> attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), null, attributes);
        }

        public static IHtmlString Image(string path, string alt, object attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), alt, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, string alt, IDictionary<string, object> attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(new Uri(path, UriKind.RelativeOrAbsolute), alt, attributes);
        }

        public static IHtmlString Image(Uri imageUri)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(imageUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(Uri imageUri, string alt)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(imageUri, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(Uri imageUri, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(imageUri, null, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(Uri imageUri, IDictionary<string, object> attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(imageUri, null, attributes);
        }

        public static IHtmlString Image(Uri imageUri, string alt, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(imageUri, alt, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(Uri imageUri, string alt, IDictionary<string, object> attributes)
        {
            Require.NotNull(imageUri, "imageUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", imageUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(alt))
            {
                tag.MergeAttribute("alt", alt);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            Contract.Assume(tag.Attributes != null);

            if (tag.Attributes.ContainsKey("alt") && !tag.Attributes.ContainsKey("title"))
            {
                tag.MergeAttribute("title", (tag.Attributes["alt"] ?? String.Empty).ToString());
            }

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Link()

        public static IHtmlString Link(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(new Uri(path, UriKind.RelativeOrAbsolute), null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(new Uri(path, UriKind.RelativeOrAbsolute), linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType, string relation)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(new Uri(path, UriKind.RelativeOrAbsolute), linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            object attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(new Uri(path, UriKind.RelativeOrAbsolute), linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(new Uri(path, UriKind.RelativeOrAbsolute), linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(Uri linkUri)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(linkUri, null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(Uri linkUri, string linkType)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(Uri linkUri, string linkType, string relation)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            Uri linkUri,
            string linkType,
            string relation,
            object attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(linkUri, linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(
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

        #endregion

        #region Script()

        public static IHtmlString Script(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(new Uri(path, UriKind.RelativeOrAbsolute), null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(new Uri(path, UriKind.RelativeOrAbsolute), scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType, object attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(new Uri(path, UriKind.RelativeOrAbsolute), scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Script(string path, string scriptType, IDictionary<string, object> attributes)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(new Uri(path, UriKind.RelativeOrAbsolute), scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Script(Uri scriptUri)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(scriptUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(Uri scriptUri, string scriptType)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(scriptUri, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(Uri scriptUri, string scriptType, object attributes)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(scriptUri, scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Script(Uri scriptUri, string scriptType, IDictionary<string, object> attributes)
        {
            Require.NotNull(scriptUri, "scriptUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", scriptUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(scriptType))
            {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            return tag.ToHtmlString();
        }

        #endregion
    }
}

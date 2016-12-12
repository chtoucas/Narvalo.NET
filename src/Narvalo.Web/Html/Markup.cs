﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    // REVIEW: Null media.
    public static class Markup
    {
        #region Image()

        public static IHtmlString Image(string path)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Image(path, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, string alt)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Image(path, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, object attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Image(path, null, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, IDictionary<string, object> attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Image(path, null, attributes);
        }

        public static IHtmlString Image(string path, string alt, object attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Image(path, alt, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, string alt, IDictionary<string, object> attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return new HtmlString(ImageCore(path, alt, attributes));
        }

        #endregion

        #region Link()

        public static IHtmlString Link(string path)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Link(path, null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Link(path, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType, string relation)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Link(path, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            object attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Link(path, linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return new HtmlString(LinkCore(path, linkType, relation, attributes));
        }

        #endregion

        #region Script()

        public static IHtmlString Script(string path)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Script(path, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Script(path, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType, object attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return Script(path, scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Script(string path, string scriptType, IDictionary<string, object> attributes)
        {
            Expect.NotNullOrEmpty(path);
            Warrant.NotNull<IHtmlString>();

            return new HtmlString(ScriptCore(path, scriptType, attributes));
        }

        #endregion

        internal static string ImageCore(string path, string alt, IDictionary<string, object> attributes)
        {
            Require.NotNullOrEmpty(path, nameof(path));
            Warrant.NotNull<string>();

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", path);

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

            var retval = tag.ToString(TagRenderMode.SelfClosing);
            Contract.Assume(retval != null);

            return retval;
        }

        internal static string LinkCore(
            string path,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Require.NotNullOrEmpty(path, nameof(path));
            Warrant.NotNull<string>();

            var tag = new TagBuilder("link");
            tag.MergeAttribute("href", path);

            if (!String.IsNullOrEmpty(linkType))
            {
                tag.MergeAttribute("type", linkType);
            }

            if (!String.IsNullOrEmpty(relation))
            {
                tag.MergeAttribute("rel", relation);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            var retval = tag.ToString(TagRenderMode.SelfClosing);
            Contract.Assume(retval != null);

            return retval;
        }

        internal static string ScriptCore(string path, string scriptType, IDictionary<string, object> attributes)
        {
            Require.NotNullOrEmpty(path, nameof(path));
            Warrant.NotNull<string>();

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", path);

            if (!String.IsNullOrEmpty(scriptType))
            {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, replaceExisting: true);

            return tag.ToString();
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(path, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, string alt)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(path, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(string path, object attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(path, null, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, IDictionary<string, object> attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(path, null, attributes);
        }

        public static IHtmlString Image(string path, string alt, object attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(path, alt, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Image(string path, string alt, IDictionary<string, object> attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return new HtmlString(ImageCore(path, alt, attributes));
        }

        #endregion

        #region Link()

        public static IHtmlString Link(string path)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(path, null, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(path, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(string path, string linkType, string relation)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(path, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            object attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(path, linkType, relation, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Link(
            string path,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return new HtmlString(LinkCore(path, linkType, relation, attributes));
        }

        #endregion

        #region Script()

        public static IHtmlString Script(string path)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(path, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(path, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(string path, string scriptType, object attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(path, scriptType, new RouteValueDictionary(attributes));
        }

        public static IHtmlString Script(string path, string scriptType, IDictionary<string, object> attributes)
        {
            Demand.NotNullOrEmpty(path);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return new HtmlString(ScriptCore(path, scriptType, attributes));
        }

        #endregion

        internal static string ImageCore(string path, string alt, IDictionary<string, object> attributes)
        {
            Require.NotNullOrEmpty(path, "path");
            Contract.Ensures(Contract.Result<string>() != null);

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
            Require.NotNullOrEmpty(path, "path");
            Contract.Ensures(Contract.Result<string>() != null);

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
            Require.NotNullOrEmpty(path, "path");
            Contract.Ensures(Contract.Result<string>() != null);

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

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Narvalo.Web.Html.Internal;

    /// <summary>
    /// Provides extension methods for <see cref="HtmlHelper"/>.
    /// </summary>
    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        public static IHtmlString LoremIpsum(this HtmlHelper @this)
        {
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            string ipsum = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            return new HtmlString(ipsum);
        }

        private static IDictionary<string, object> ObjectToDictionary_(object value)
        {
            return new RouteValueDictionary(value);
        }
    }

    // Image extensions.
    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(@this, imageUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(@this, imageUri, alt, (IDictionary<string, object>)null);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(@this, imageUri, null, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, IDictionary<string, object> attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(@this, imageUri, null, attributes);
        }

        public static IHtmlString Image(this HtmlHelper @this, Uri imageUri, string alt, object attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Image(@this, imageUri, alt, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Image(
            this HtmlHelper @this,
            Uri imageUri,
            string alt,
            IDictionary<string, object> attributes)
        {
            Contract.Requires(imageUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return ImageHelper_(@this, imageUri, alt, ObjectToDictionary_(attributes));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        private static IHtmlString ImageHelper_(
            this HtmlHelper @this,
            Uri imageUri,
            string alt,
            IDictionary<string, object> attributes)
        {
            Require.NotNull(imageUri, "imageUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", imageUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(alt))
            {
                tag.MergeAttribute("alt", alt);
            }

            tag.MergeAttributes(attributes, true);

            Contract.Assume(tag.Attributes != null);

            if (tag.Attributes.ContainsKey("alt") && !tag.Attributes.ContainsKey("title"))
            {
                tag.MergeAttribute("title", (tag.Attributes["alt"] ?? String.Empty).ToString());
            }

            return tag.ToHtmlString(TagRenderMode.SelfClosing);
        }
    }

    // Link extensions.
    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(@this, linkUri, linkType, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(this HtmlHelper @this, Uri linkUri, string linkType, string relation)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(@this, linkUri, linkType, relation, (IDictionary<string, object>)null);
        }

        public static IHtmlString Link(
            this HtmlHelper @this,
            Uri linkUri,
            string linkType,
            string relation,
            object attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Link(@this, linkUri, linkType, relation, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Link(
            this HtmlHelper @this,
            Uri linkUri,
            string linkType,
            string relation,
            IDictionary<string, object> attributes)
        {
            Contract.Requires(linkUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return LinkHelper_(@this, linkUri, linkType, relation, attributes);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        private static IHtmlString LinkHelper_(
            this HtmlHelper @this,
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
    }

    // Script extensions.
    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(@this, scriptUri, null, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri, string scriptType)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(@this, scriptUri, scriptType, (IDictionary<string, object>)null);
        }

        public static IHtmlString Script(this HtmlHelper @this, Uri scriptUri, string scriptType, object attributes)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return Script(@this, scriptUri, scriptType, ObjectToDictionary_(attributes));
        }

        public static IHtmlString Script(
            this HtmlHelper @this,
            Uri scriptUri,
            string scriptType,
            IDictionary<string, object> attributes)
        {
            Contract.Requires(scriptUri != null);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            return ScriptHelper_(@this, scriptUri, scriptType, attributes);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "[Intentionally] We use an extension method to improve the accessibility of this method.")]
        private static IHtmlString ScriptHelper_(
            this HtmlHelper @this,
            Uri scriptUri,
            string scriptType,
            IDictionary<string, object> attributes)
        {
            Require.NotNull(scriptUri, "scriptUri");
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", scriptUri.ToProtocolRelativeString());

            if (!String.IsNullOrEmpty(scriptType))
            {
                tag.MergeAttribute("type", scriptType);
            }

            tag.MergeAttributes(attributes, true);

            return tag.ToHtmlString();
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Narvalo.Web.UI;

    public static class HtmlHelperExtensions
    {
        public static void RenderCss(this HtmlHelper @this, string relativePath)
        {
            Acknowledge.Object(@this);
            Acknowledge.NotNullOrEmpty(relativePath);

            Render_(@this, LinkMarkup_(relativePath, null /* linkType */, Asset.InternalCssRelation, null));
        }

        public static void RenderCss(this HtmlHelper @this, string relativePath, string media)
        {
            Acknowledge.Object(@this);
            Acknowledge.NotNullOrEmpty(relativePath);

            Render_(@this, LinkMarkup_(relativePath, null /* linkType */, Asset.InternalCssRelation, new { media = media }));
        }

        public static void RenderImage(this HtmlHelper @this, string relativePath, string alt)
        {
            Acknowledge.Object(@this);
            Acknowledge.NotNullOrEmpty(relativePath);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            Render_(@this, Markup.ImageInternal(assetPath, alt, null));
        }

        public static void RenderJavaScript(this HtmlHelper @this, string relativePath)
        {
            Acknowledge.NotNullOrEmpty(relativePath);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            Render_(@this, Markup.ScriptInternal(assetPath, null, null));
        }

        public static void RenderLess(this HtmlHelper @this, string relativePath)
        {
            Acknowledge.Object(@this);
            Acknowledge.NotNullOrEmpty(relativePath);

            Render_(@this, LinkMarkup_(relativePath, Asset.InternalLessLinkType, Asset.InternalLessRelation, null));
        }

        public static void RenderLess(this HtmlHelper @this, string relativePath, string media)
        {
            Acknowledge.Object(@this);
            Acknowledge.NotNullOrEmpty(relativePath);

            Render_(@this, LinkMarkup_(relativePath, Asset.InternalLessLinkType, Asset.InternalLessRelation, new { media = media }));
        }

        private static void Render_(HtmlHelper @this, string content)
        {
            Require.Object(@this);

            @this.ViewContext.Writer.Write(content);
        }

        private static string LinkMarkup_(
            string relativePath,
            string linkType,
            string relation,
            object attributes)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<string>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.LinkInternal(assetPath, linkType, relation, new RouteValueDictionary(attributes));
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Web.UI;

    public static class Asset
    {
        private const string CSS_RELATION = "stylesheet";
        private const string LESS_LINK_TYPE = "text/css";
        private const string LESS_RELATION = "stylesheet/less";

        public static IHtmlString Css(string relativePath)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, null /* linkType */, CSS_RELATION);
        }

        public static IHtmlString Css(string relativePath, string media)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, null /* linkType */, CSS_RELATION, new { media = media });
        }

        public static IHtmlString Image(string relativePath, string alt)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetImageUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Image(assetPath, alt);
        }

        public static IHtmlString JavaScript(string relativePath)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Script(assetPath);
        }

        public static IHtmlString Less(string relativePath)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, LESS_LINK_TYPE, LESS_RELATION);
        }

        public static IHtmlString Less(string relativePath, string media)
        {
            Demand.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, LESS_LINK_TYPE, LESS_RELATION, new { media = media });
        }
    }
}

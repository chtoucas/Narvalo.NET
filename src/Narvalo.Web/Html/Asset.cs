// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Web.UI;

    public static class Asset
    {
        internal const string InternalCssRelation = "stylesheet";
        internal const string InternalLessLinkType = "text/css";
        internal const string InternalLessRelation = "stylesheet/less";

        public static IHtmlString Css(string relativePath)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, null /* linkType */, InternalCssRelation);
        }

        public static IHtmlString Css(string relativePath, string media)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, null /* linkType */, InternalCssRelation, new { media = media });
        }

        public static IHtmlString Image(string relativePath, string alt)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetImageUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Image(assetPath, alt);
        }

        public static IHtmlString JavaScript(string relativePath)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Script(assetPath);
        }

        public static IHtmlString Less(string relativePath)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, InternalLessLinkType, InternalLessRelation);
        }

        public static IHtmlString Less(string relativePath, string media)
        {
            Acknowledge.NotNullOrEmpty(relativePath);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            var assetPath = UrlManip.ToProtocolRelativeString(assetUri);

            return Markup.Link(assetPath, InternalLessLinkType, InternalLessRelation, new { media = media });
        }
    }
}

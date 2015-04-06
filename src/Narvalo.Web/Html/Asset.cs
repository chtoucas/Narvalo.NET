// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Web.UI;

    public static class Asset
    {
        public static IHtmlString Css(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            return Markup.Link(assetUri, null /* linkType */, "stylesheet");
        }

        public static IHtmlString Css(string relativePath, string media)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            return Markup.Link(assetUri, null /* linkType */, "stylesheet", new { media = media });
        }

        public static IHtmlString Image(string relativePath, string alt)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetImageUri(relativePath);
            return Markup.Image(assetUri, alt);
        }

        public static IHtmlString JavaScript(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScriptUri(relativePath);
            return Markup.Script(assetUri);
        }

        public static IHtmlString Less(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            return Markup.Link(assetUri, "text/css", "stylesheet/less");
        }

        public static IHtmlString Less(string relativePath, string media)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyleUri(relativePath);
            return Markup.Link(assetUri, "text/css", "stylesheet/less", new { media = media });
        }
    }
}

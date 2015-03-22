// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Html
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    using Narvalo.Web.UI.Assets;

    public sealed class AssetHelper
    {
        private readonly HtmlHelper _htmlHelper;

        public AssetHelper(HtmlHelper htmlHelper)
        {
            Require.NotNull(htmlHelper, "htmlHelper");

            _htmlHelper = htmlHelper;
        }

        public IHtmlString Css(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, null /* linkType */, "stylesheet");
        }

        public IHtmlString Css(string relativePath, string media)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, null /* linkType */, "stylesheet", new { media = media });
        }

        public IHtmlString Image(string relativePath, string alt)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetImage(relativePath);
            return _htmlHelper.Image(assetUri, alt);
        }

        public IHtmlString JavaScript(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetScript(relativePath);
            return _htmlHelper.Script(assetUri);
        }

        public IHtmlString Less(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, "text/css", "stylesheet/less");
        }

        public IHtmlString Less(string relativePath, string media)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<IHtmlString>() != null);

            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, "text/css", "stylesheet/less", new { media = media });
        }

#if CONTRACTS_FULL && !CODE_ANALYSIS // [Ignore] Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_htmlHelper != null);
        }

#endif
    }
}

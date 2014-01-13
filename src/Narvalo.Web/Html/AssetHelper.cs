namespace Narvalo.Web.Html
{
    using System.Web;
    using System.Web.Mvc;
    using Narvalo.Web.UI.Assets;

    public class AssetHelper
    {
        HtmlHelper _htmlHelper;

        public AssetHelper(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public IHtmlString Css(string relativePath)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, null /* linkType */, "stylesheet");
        }

        public IHtmlString Css(string relativePath, string media)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, null /* linkType */, "stylesheet", new { media = media });
        }

        public IHtmlString Image(string relativePath, string alt)
        {
            var assetUri = AssetManager.GetImage(relativePath);
            return _htmlHelper.Image(assetUri, alt);
        }

        public IHtmlString JavaScript(string relativePath)
        {
            var assetUri = AssetManager.GetScript(relativePath);
            return _htmlHelper.Script(assetUri);
        }

        public IHtmlString Less(string relativePath)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, "text/css", "stylesheet/less");
        }

        public IHtmlString Less(string relativePath, string media)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return _htmlHelper.Link(assetUri, "text/css", "stylesheet/less", new { media = media });
        }
    }
}

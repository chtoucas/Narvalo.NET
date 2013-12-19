namespace Narvalo.Web.Html
{
    using System.Web;
    using Narvalo.Web.UI.Assets;

    public partial class Tag
    {
        public IHtmlString CssAsset(string relativePath)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return Link(assetUri, null /* linkType */, "stylesheet");
        }

        public IHtmlString CssAsset(string relativePath, string media)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return Link(assetUri, null /* linkType */, "stylesheet", new { media = media });
        }

        public IHtmlString ImageAsset(string relativePath, string alt)
        {
            var assetUri = AssetManager.GetImage(relativePath);
            return Image(assetUri, alt);
        }

        public IHtmlString JavaScriptAsset(string relativePath)
        {
            var assetUri = AssetManager.GetScript(relativePath);
            return Script(assetUri);
        }
    }
}

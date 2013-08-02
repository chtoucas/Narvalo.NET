namespace Narvalo.Web.Html
{
    using System.Web;
    using Narvalo.Web.UI.Assets;

    public static class AssetTag
    {
        public static IHtmlString Css(string relativePath)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return Tag.Link(assetUri, null /* linkType */, "stylesheet");
        }

        public static IHtmlString Css(string relativePath, string media)
        {
            var assetUri = AssetManager.GetStyle(relativePath);
            return Tag.Link(assetUri, null /* linkType */, "stylesheet", new { media = media });
        }

        public static IHtmlString Image(string relativePath, string alt)
        {
            var assetUri = AssetManager.GetImage(relativePath);
            return Tag.Image(assetUri, alt);
        }

        public static IHtmlString JavaScript(string relativePath)
        {
            var assetUri = AssetManager.GetScript(relativePath);
            return Tag.Script(assetUri);
        }
    }
}

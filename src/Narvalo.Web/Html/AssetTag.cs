namespace Narvalo.Web.Html
{
    using System.Web;
    using Narvalo.Web.UI.Assets;

    public static class AssetTag
    {
        public static IHtmlString Css(string relativePath)
        {
            var asset = AssetManager.GetStyle(relativePath);
            return Tag.Link(asset.Url, null /* linkType */, "stylesheet");
        }

        public static IHtmlString Css(string relativePath, string media)
        {
            var asset = AssetManager.GetStyle(relativePath);
            return Tag.Link(asset.Url, null /* linkType */, "stylesheet", new { media = media });
        }

        public static IHtmlString Image(string relativePath, string alt)
        {
            var asset = AssetManager.GetImage(relativePath);
            return Tag.Image(asset.Url, alt);
        }

        public static IHtmlString JavaScript(string relativePath)
        {
            var asset = AssetManager.GetScript(relativePath);
            return Tag.Script(asset.Url);
        }
    }
}

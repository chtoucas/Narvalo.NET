namespace Narvalo.Web.Html
{
    using System.Text;
    using System.Web;
    using Narvalo.Web.UI.Assets;

    public static class AssetHelperExtensions
    {
        public static IHtmlString Css(this AssetHelper @this, AssetBundle bundle, bool debug)
        {
            IHtmlString result;

            if (debug) {
                var sb = new StringBuilder();
                foreach (var path in bundle.RelativePaths) {
                    sb.Append(@this.Css(path));
                }
                result = new HtmlString(sb.ToString());
            }
            else {
                result = @this.Css(bundle.BundlePath);
            }

            return result;
        }

        public static IHtmlString JavaScript(this AssetHelper @this, AssetBundle bundle, bool debug)
        {
            IHtmlString result;

            if (debug) {
                var sb = new StringBuilder();
                foreach (var path in bundle.RelativePaths) {
                    sb.Append(@this.JavaScript(path));
                }
                result = new HtmlString(sb.ToString());
            }
            else {
                result = @this.JavaScript(bundle.BundlePath);
            }

            return result;
        }
    }
}

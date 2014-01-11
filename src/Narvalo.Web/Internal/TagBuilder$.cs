namespace Narvalo.Web.Internal
{
    using System.Web;
    using System.Web.Mvc;

    static class TagBuilderExtensions
    {
        public static IHtmlString ToHtmlString(this TagBuilder @this)
        {
            return @this.ToHtmlString(TagRenderMode.Normal);
        }

        public static IHtmlString ToHtmlString(this TagBuilder @this, TagRenderMode renderMode)
        {
            return new HtmlString(@this.ToString(renderMode));
        }

    }
}

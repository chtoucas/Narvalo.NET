namespace Narvalo.Web.Internal
{
    using System.Web;
    using System.Web.Mvc;
    using Narvalo;

    static class TagBuilderExtensions
    {
        public static IHtmlString ToHtmlString(this TagBuilder @this)
        {
            Requires.Object(@this);

            return @this.ToHtmlString(TagRenderMode.Normal);
        }

        public static IHtmlString ToHtmlString(this TagBuilder @this, TagRenderMode renderMode)
        {
            Requires.Object(@this);

            return new HtmlString(@this.ToString(renderMode));
        }

    }
}

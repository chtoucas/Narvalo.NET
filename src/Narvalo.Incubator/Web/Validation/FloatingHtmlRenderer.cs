namespace Narvalo.Web.Validation
{
    /// <summary>
    /// HTML implementation of the rendering engine
    /// </summary>
    public class FloatingHtmlRenderer : InlineHtmlRenderer
    {
        public FloatingHtmlRenderer()
        {
            // change the DivStyle to floating - easy peasy!
            DivStyle = "position:absolute;top:0px;left:0px;z-index:9999;clear:both;padding:10px;";
        }
    }
}

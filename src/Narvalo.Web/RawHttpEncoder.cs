namespace Narvalo.Web
{
    using System.IO;
    using System.Web.Util;

    // WARNING: Cet encodeur ne doit pas être utilisé dans un site web ouvert à la modification non controlée,
    // et même dans ce cas...
    public class RawHttpEncoder : HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(value);
        }

        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(value);
        }
    }
}
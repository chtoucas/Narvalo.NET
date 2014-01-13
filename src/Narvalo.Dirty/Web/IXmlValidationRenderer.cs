namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;

    public interface IXmlValidationRenderer
    {
        void Render(HttpContext context, IEnumerable<ValidationEventArgs> errors);
    }
}

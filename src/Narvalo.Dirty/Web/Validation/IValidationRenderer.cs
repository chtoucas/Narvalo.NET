namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;

    public interface IValidationRenderer
    {
        void Render(HttpContext context, IList<ValidationEventArgs> errors);
    }
}

namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;
    using Serilog;

    public class SerilogValidationRenderer : IValidationRenderer
    {
        #region IValidationRenderer

        public void Render(HttpContext context, IList<ValidationEventArgs> errors)
        {
            foreach (var err in errors) {
                switch (err.Severity) {
                    case XmlSeverityType.Error:
                        Log.Error(err.ToString());
                        break;
                    case XmlSeverityType.Warning:
                        Log.Warning(err.ToString());
                        break;
                }
            }
        }

        #endregion
    }
}

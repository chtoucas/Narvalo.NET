using Serilog;

namespace Narvalo.Extras.Serilog
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;
    using Narvalo.Web;

    public class SerilogXmlValidationRenderer : IXmlValidationRenderer
    {
        #region IValidationRenderer

        public void Render(HttpContext context, IEnumerable<ValidationEventArgs> errors)
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

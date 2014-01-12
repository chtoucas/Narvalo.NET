namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;
    using Narvalo.Diagnostics;

    public class LoggingRenderer : IValidationRenderer
    {
        private readonly ILogger _logger;

        public LoggingRenderer(ILogger logger)
        {
            _logger = logger;
        }

        #region IValidationRenderer

        public void Render(HttpContext context, IList<ValidationEventArgs> errors)
        {
            foreach (var err in errors) {
                switch (err.Severity) {
                    case XmlSeverityType.Error:
                        _logger.Log(LoggerLevel.Error, err.ToString());
                        break;
                    case XmlSeverityType.Warning:
                        _logger.Log(LoggerLevel.Warning ,err.ToString());
                        break;
                }
            }
        }

        #endregion
    }
}

namespace Narvalo.Web
{
    using System;
    using System.Web;
    using Narvalo;
    using Serilog.Core;
    using Serilog.Events;

    [CLSCompliant(false)]
    public class HttpLogEventEnricher : ILogEventEnricher
    {
        #region ILogEventEnricher

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Requires.NotNull(logEvent, "logEvent");
            Requires.NotNull(propertyFactory, "propertyFactory");

            if (HttpContext.Current == null) {
                return;
            }

            var req = HttpContext.Current.Request;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Domain", req.Url.Host));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RawUrl", req.RawUrl));
            if (req.UrlReferrer != null) {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UrlReferrer", req.UrlReferrer.ToString()));
            }
            
            if (!String.IsNullOrEmpty(req.UserHostAddress)) {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserHostAddress", req.UserHostAddress));
            }
            
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserAgent", req.UserAgent));
        }

        #endregion
    }
}

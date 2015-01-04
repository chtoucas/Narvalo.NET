// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Externs.Serilog
{
    using System;
    using System.Web;
    using Narvalo;
    using global::Serilog.Core;
    using global::Serilog.Events;

    [CLSCompliant(false)]
    public sealed class HttpLogEventEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Require.NotNull(logEvent, "logEvent");
            Require.NotNull(propertyFactory, "propertyFactory");

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
    }
}

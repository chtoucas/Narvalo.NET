// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Serilog.Events;

    public sealed class AppSettings
    {
        static readonly LogEventLevel DefaultLogMinimumLevel_ = LogEventLevel.Information;

        AppSettings() { }

        [CLSCompliant(false)]
        public LogEventLevel LogMinimumLevel { get; private set; }

        public static AppSettings FromConfiguration()
        {
            var nvc = Filter_(ConfigurationManager.AppSettings);

            return Create(nvc);
        }

        internal static AppSettings Create(NameValueCollection nvc)
        {
            Require.NotNull(nvc, "nvc");

            var self = new AppSettings();
            self.Initialize_(nvc);
            return self;
        }

        static NameValueCollection Filter_(NameValueCollection settings)
        {
            var keys = settings.AllKeys
                .Where(_ => _.StartsWith("narrative:", StringComparison.OrdinalIgnoreCase));

            var nvc = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (var key in keys) {
                nvc[key] = settings[key];
            }

            return nvc;
        }

        void Initialize_(NameValueCollection nvc)
        {
            LogMinimumLevel = nvc.MayGetSingle("narrative:LogMinimumLevel")
                .Select(ParseTo.Enum<LogEventLevel>)
                .ToNullable()
                ?? DefaultLogMinimumLevel_;
        }
    }
}

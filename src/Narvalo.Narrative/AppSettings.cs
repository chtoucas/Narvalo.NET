// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Serilog.Events;

    public sealed class AppSettings
    {
        static readonly Func<string> DefaultOutputDirectory_
            = () =>
            {
                var execDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(execDirectory, "docs");
            };

        static readonly LogEventLevel DefaultLogMinimumLevel_ = LogEventLevel.Information;

        AppSettings() { }

        public string OutputDirectory { get; private set; }

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
            OutputDirectory = nvc.MayGetSingle("narrative:OutputDirectory")
                .ValueOrElse(DefaultOutputDirectory_);

            LogMinimumLevel = nvc.MayGetSingle("narrative:LogMinimumLevel")
                .Select(ParseTo.Enum<LogEventLevel>)
                .ToNullable()
                ?? DefaultLogMinimumLevel_;
        }
    }
}

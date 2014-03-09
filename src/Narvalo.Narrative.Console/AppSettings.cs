// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Serilog.Events;

    public sealed class AppSettings
    {
        const string SettingPrefix_ = "narrative:";

        static readonly string ExecutingDirectory_
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public string LogProfile { get; set; }
        [CLSCompliant(false)]
        public LogEventLevel LogMinimumLevel { get; set; }
        public string OutputDirectory { get; set; }

        AppSettings() { }

        public static AppSettings FromConfiguration()
        {
            return (new AppSettings()).Load();
        }

        public AppSettings Load()
        {
            LoadSettings_(ConfigurationManager.AppSettings);

            return this;
        }

        void LoadSettings_(NameValueCollection appSettings)
        {
            var keys = appSettings.AllKeys
                .Where(_ => _.StartsWith(SettingPrefix_, StringComparison.OrdinalIgnoreCase));

            var settings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (var key in keys) {
                settings[key] = appSettings[key];
            }

            Initialize_(settings);
        }

        void Initialize_(NameValueCollection source)
        {
            LogProfile = source.MayGetSingle("narrative:LogProfile")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'narrative:LogProfile'."));

            LogMinimumLevel = source.MayGetSingle("narrative:LogMinimumLevel")
                .Select(ParseTo.Enum<LogEventLevel>)
                .UnpackOrThrow(
                    () => new ConfigurationErrorsException("Missing or invalid config 'narrative:LogMinimumLevel'."));

            OutputDirectory = source.MayGetSingle("narrative:OutputDirectory")
                .ValueOrElse(Path.Combine(ExecutingDirectory_, "docs"));
        }
    }
}

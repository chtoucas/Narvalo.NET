// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Serilog.Events;

    public static class SettingsManager
    {
        [CLSCompliant(false)]
        public static Settings Resolve()
        {
            var settings = new Settings();

            Amend(settings, GetAppSettings_());
            Amend(settings, GetArguments_());

            return settings;
        }

        [CLSCompliant(false)]
        public static Settings FromConfiguration()
        {
            var settings = new Settings();

            Amend(settings, GetAppSettings_());

            return settings;
        }

        internal static void Amend(Settings settings, NameValueCollection nvc)
        {
            DebugCheck.NotNull(settings);
            DebugCheck.NotNull(nvc);

            nvc.MayGetSingle("narrative:RunInParallel")
                .Select(ParseTo.Boolean)
                .ToNullable()
                .OnValue(_ => { settings.RunInParallel = _; });

            nvc.MayGetSingle("narrative:LogMinimumLevel")
                .Select(ParseTo.Enum<LogEventLevel>)
                .ToNullable()
                .OnValue(_ => { settings.LogMinimumLevel = _; });

            nvc.MayGetSingle("narrative::OutputDirectory")
                .OnSome(_ => { settings.OutputDirectory = _; });
        }

        // FIXME
        internal static void Amend(Settings settings, Arguments arguments)
        {
            DebugCheck.NotNull(settings);
            DebugCheck.NotNull(arguments);

            if (arguments.DryRun) {
                settings.DryRun = arguments.DryRun;
            }
        }

        static Arguments GetArguments_()
        {
            var self = new Arguments();
            var args = Environment.GetCommandLineArgs();

            var parser = new CommandLine.Parser();
            parser.ParseArguments(args, self);

            return self;
        }

        static NameValueCollection GetAppSettings_()
        {
           return Filter_(ConfigurationManager.AppSettings);
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
    }
}

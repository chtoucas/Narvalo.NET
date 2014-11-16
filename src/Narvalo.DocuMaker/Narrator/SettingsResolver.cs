// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Narrator
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.DocuMaker.Properties;
    using Narvalo.Fx;
    using Serilog.Events;

    public static class SettingsResolver
    {
        public static Settings Resolve()
        {
            var settings = new Settings();

            AmendWithAppSettings_(settings, ParseAppSettings_());
            AmendWithCmdLineArguments_(settings, ParseCmdLineArguments_());
            AddDefaultValuesIfNeeded_(settings);

            return settings;
        }

        static void AddDefaultValuesIfNeeded_(Settings settings)
        {
            if (String.IsNullOrWhiteSpace(settings.OutputDirectory)) {
                settings.OutputDirectory = GetDefaultOutputDirectory_();
            }
        }

        static void AmendWithAppSettings_(Settings settings, NameValueCollection nvc)
        {
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

        static void AmendWithCmdLineArguments_(Settings settings, Arguments arguments)
        {
            DebugCheck.NotNull(arguments);

            if (arguments.DryRunSet) {
                settings.DryRun = arguments.DryRun;
            }

            if (arguments.RunInParallelSet) {
                settings.RunInParallel = arguments.RunInParallel;
            }

            settings.Path = arguments.Path;
        }

        static string GetDefaultOutputDirectory_()
        {
            var execDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return Path.Combine(execDirectory, "docs");
        }

        static NameValueCollection ParseAppSettings_()
        {
            return Filter_(ConfigurationManager.AppSettings);
        }

        static Arguments ParseCmdLineArguments_()
        {
            var self = new Arguments();
            var args = Environment.GetCommandLineArgs();

            var parser = new CommandLine.Parser();
            if (!parser.ParseArgumentsStrict(args, self)) {
                throw new DocuMakerException(Resources.CommandLineException);
            }

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
    }
}

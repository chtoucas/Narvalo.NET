// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using System;
    using Narvalo.Narrative.Configuration;
    using Narvalo.Narrative.Properties;
    using Serilog;
    using Serilog.Events;

    public sealed class SerilogConfig
    {
        readonly Settings _settings;

        public SerilogConfig(Settings settings)
        {
            _settings = settings;
        }

        public void Configure()
        {
            Log.Logger = CreateLogger_(_settings.LogMinimumLevel);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;
        }

        static ILogger CreateLogger_(LogEventLevel mimimumLevel)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Is(mimimumLevel)
               .WriteTo.ColoredConsole()
               .CreateLogger();
        }

        static void OnUnhandledException_(object sender, UnhandledExceptionEventArgs args)
        {
            try {
                Log.Fatal(Resources.UnhandledException, (Exception)args.ExceptionObject);
            }
            finally {
                Environment.Exit(1);
            }
        }
    }
}

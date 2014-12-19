// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose
{
    using System;
    using Prose.Narrator;
    using Prose.Properties;
    using Serilog;
    using Serilog.Events;

    public static class Program
    {
        const int SuccessfulExitCode_ = 0;
        const int ErrorExitCode_ = 1;
        const int FatalExitCode_ = 2;

        public static int Main()
        {
            // Resolve settings.
            var settings = SettingsResolver.Resolve();

            // Configure logging.
            Log.Logger = CreateLogger_(settings.LogMinimumLevel);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;

            try {
                new Application(settings).Execute();
            }
            catch (ProseException ex) {
                Log.Error(Resources.UnhandledNarrativeException, ex);

                return ErrorExitCode_;
            }

            return SuccessfulExitCode_;
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
                Environment.Exit(FatalExitCode_);
            }
        }
    }
}

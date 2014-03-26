// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using Narvalo.Narrative.Narrator;
    using Narvalo.Narrative.Properties;
    using Serilog;

    public static class Program
    {
        const int SuccessfulExitCode_ = 0;
        const int ErrorExitCode_ = 1;
        const int FatalExitCode_ = 2;

        public static int Main(string[] args)
        {
            // Resolve settings.
            var settings = SettingsResolver.Resolve();

            // Configure logging.
            (new SerilogConfig(settings.LogMinimumLevel)).Configure();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;

            try {
                new Application(settings).Execute();
            }
            catch (NarrativeException ex) {
                Log.Error(Resources.UnhandledNarrativeException, ex);

                return ErrorExitCode_;
            }

            return SuccessfulExitCode_;
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

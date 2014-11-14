// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Playground.Properties;
    using Serilog;
    using Serilog.Events;

    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Log.Logger = CreateLogger_();
            Log.Information(Resources.Starting);

            new SampleCommand().Execute();
            new SampleCommand().Execute();

            Log.Information(Resources.Ending);
        }

        static ILogger CreateLogger_()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .WriteTo.Trace()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try {
                Log.Fatal(Resources.UnhandledException, (Exception)e.ExceptionObject);
            }
            finally {
                Environment.Exit(1);
            }
        }
    }
}

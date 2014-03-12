// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using Autofac;
    using Narvalo.Narrative.Properties;
    using Serilog;
    using Serilog.Events;

    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var settings = AppSettings.FromConfiguration();

            ConfigureLogging_(settings.LogMinimumLevel);

            Log.Information(Resources.Starting);

            try {
                var options = CommandOptions.Parse(args);

                using (var container = CreateContainer_(settings)) {
                    container.Resolve<Command>().Run(options);
                }
            }
            catch (NarrativeException ex) {
                Log.Fatal(Resources.NarrativeException, ex);
            }

            Log.Information(Resources.Ending);
        }

        static IContainer CreateContainer_(AppSettings settings)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CommandModule(settings));
            return builder.Build();
        }

        static void ConfigureLogging_(LogEventLevel mimimumLevel)
        {
            Log.Logger = CreateLogger_(mimimumLevel);

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

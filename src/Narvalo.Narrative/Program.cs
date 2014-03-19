// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Diagnostics;
    using Autofac;
    using Narvalo.Narrative.Properties;
    using NodaTime;
    using Serilog;
    using Serilog.Events;

    public sealed class Program : CommandLine<AppArguments>
    {
        public Program(AppArguments arguments)
            : base(arguments) { }

        public static void Main(string[] args)
        {
            var settings = AppSettings.FromConfiguration();

            SetupLogging_(settings.LogMinimumLevel);

            Log.Verbose(Resources.Starting);

            try {
                var arguments = AppArguments.Parse(args);

                if (arguments.DryRun) {
                    Log.Warning(Resources.DryRun);
                }

                new Program(arguments).Run();
            }
            catch (NarrativeException ex) {
                Log.Fatal(Resources.UnhandledNarrativeException, ex);
            }

            Log.Verbose(Resources.Ending);
        }

        public override void Run()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AppModule(Arguments));

            using (var container = builder.Build()) {
                var stopWatch = Stopwatch.StartNew();

                container.Resolve<IRunner>().Run();

                var elapsedTime = Duration.FromTicks(stopWatch.Elapsed.Ticks);
                Log.Verbose(Resources.ElapsedTime, elapsedTime);
            }
        }

        static void SetupLogging_(LogEventLevel mimimumLevel)
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

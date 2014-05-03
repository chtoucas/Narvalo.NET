// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Autofac;
    using Narvalo.Mvp;
    using Narvalo.Mvp.CommandLine;
    using Playground.Commands;
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

            using (var container = CreateContainer_()) {
                ConfigureMvp_(container);

                new TestCommand().Execute();
                new TestCommand().Execute();
                new TestCommand().Execute();
            }

            Log.Information(Resources.Ending);
        }

        static void ConfigureMvp_(IContainer container)
        {
            var mvp = new CommandsMvpBootstrapper();
            mvp.Configuration
                .PresenterFactory.Is(new AutofacPresenterFactory(container));
            mvp.Run();
        }

        static IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();
            builder.RegisterPresenters(typeof(Program).Assembly).AsSelf();
            return builder.Build();
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

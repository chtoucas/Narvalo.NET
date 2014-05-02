// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Autofac;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Binder;
    using Playground.Commands;
    using Playground.Properties;
    using Serilog;
    using Serilog.Events;

    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            InitializeRuntime_();

            Log.Information(Resources.Starting);

            //new TestCommand().Execute();
            //new TestCommand().Execute();

            using (var container = CreateContainer_()) {
                new MvpBootstrapper()
                    .DiscoverPresenter.With(new ConventionBasedPresenterDiscoveryStrategy())
                    .PresenterFactory.Is(new AutofacPresenterFactory(container))
                    .Run();

                new TestCommand().Execute();
                new TestCommand().Execute();
            }

            Log.Information(Resources.Ending);
        }

        static IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

            builder.RegisterPresenters(typeof(Program).Assembly).AsSelf();
            builder.RegisterType<MessageBus>().As<IMessageBus>();

            return builder.Build();
        }

        static void InitializeRuntime_()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Is(LogEventLevel.Verbose)
               .WriteTo.Trace()
               .WriteTo.ColoredConsole()
               .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                try {
                    Log.Fatal(Resources.UnhandledException, (Exception)e.ExceptionObject);
                }
                finally {
                    Environment.Exit(1);
                }
            };
        }
    }
}

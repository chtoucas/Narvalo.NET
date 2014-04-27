// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Autofac;
    using Narvalo.Mvp;
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

            using (var container = CreateContainer_()) {
                new AutofacPresenterFactory(container).SelfRegister();

                new TestCommand().Execute();
            }

            Log.Information(Resources.Ending);
        }

        static IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

            builder.RegisterPresenters(typeof(Program).Assembly).AsSelf();

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
                Log.Fatal(Resources.UnhandledException, (Exception)e.ExceptionObject);
            };
        }
    }
}

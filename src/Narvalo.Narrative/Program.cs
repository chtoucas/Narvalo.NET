// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Autofac;
    using Narvalo.Narrative.Properties;
    using NodaTime;
    using Serilog;
    using Serilog.Events;

    sealed class Program : CommandLine<Arguments>
    {
        public Program(Arguments arguments) : base(arguments) { }

        static void Main(string[] args)
        {
            var settings = AppSettings.FromConfiguration();

            SetupLogging_(settings.LogMinimumLevel);

            Log.Information(Resources.Starting);

            try {
                var arguments = Narvalo.Narrative.Arguments.Parse(args);

                new Program(arguments).Run();
            }
            catch (NarrativeException ex) {
                Log.Fatal(Resources.UnhandledNarrativeException, ex);
            }

            Log.Information(Resources.Ending);
        }

        public override void Run()
        {
            if (Arguments.DryRun) {
                Log.Warning(Resources.DryRun);
            }

            var stopWatch = Stopwatch.StartNew();

            using (var container = CreateContainer_()) {
                container.Resolve<IRunner>().Run();
            }

            var elapsedTime = Duration.FromTicks(stopWatch.Elapsed.Ticks);
            Log.Information(Resources.ElapsedTime, elapsedTime);
        }

        IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.Register(CreateTemplate_).As<ITemplate>();

            if (Arguments.DryRun) {
                builder.RegisterType<DryRunWeaver>().As<IWeaver>();
            }
            else {
                builder.RegisterType<Weaver>().As<IWeaver>();
            }

            builder.Register(CreateRunner_).As<IRunner>();

            return builder.Build();
        }

        static RazorTemplate CreateTemplate_(IComponentContext context)
        {
            return new RazorTemplate(Resources.Template, context.Resolve<IMarkdownEngine>());
        }

        IRunner CreateRunner_(IComponentContext context)
        {
            IRunner runner;

            var attrs = File.GetAttributes(Arguments.Path);

            if (attrs.HasFlag(FileAttributes.Directory)) {
                var directory = new DirectoryInfo(Arguments.Path);

                if (Arguments.RunInParallel) {
                    runner = new ParallelRunner(
                        context.Resolve<IWeaver>(),
                        directory,
                        Arguments.OutputDirectory);
                }
                else {
                    runner = new SequentialRunner(
                        context.Resolve<IWeaver>(),
                        directory,
                        Arguments.OutputDirectory);
                }
            }
            else {
                runner = new Runner(
                    context.Resolve<IWeaver>(),
                    new FileInfo(Arguments.Path),
                    Arguments.OutputDirectory);
            }

            if (Arguments.DryRun) {
                runner.DryRun = true;
            }

            return runner;
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

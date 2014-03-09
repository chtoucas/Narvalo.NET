// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using Autofac;
    using Narvalo.Narrative.Properties;
    using Serilog;
    using Serilog.Events;

    public sealed class Program
    {
        readonly AppSettings _settings;

        Program(AppSettings settings)
        {
            _settings = settings;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var settings = AppSettings.FromConfiguration();

            InitializeRuntime_(settings);

            Log.Information(Resources.Starting);

            try {
                var options = ParseArguments_(args);
                new Program(settings).Run(options);
            }
            catch (Exception ex) {
                LogUnhandledException_(ex);
            }

            Log.Information(Resources.Ending);
        }

        public void Run(AppOptions options)
        {
            using (var container = CreateContainer_(_settings)) {
                container.Resolve<AppService>().Process(options.Paths);
            }
        }

        static AppOptions ParseArguments_(string[] args)
        {
            var options = new AppOptions();

            var parser = new CommandLine.Parser();
            parser.ParseArgumentsStrict(
                args,
                options,
                () => { throw new ApplicationException("Failed to process arguments."); });

            return options;
        }

        static IContainer CreateContainer_(AppSettings settings)
        {
            var builder = new ContainerBuilder();

            builder.Register(_ => settings).AsSelf().SingleInstance();

            builder.RegisterType<AppService>().AsSelf();

            return builder.Build();
        }

        static void OnUnhandledException_(object sender, UnhandledExceptionEventArgs args)
        {
            try {
                LogUnhandledException_((Exception)args.ExceptionObject);
            }
            finally {
                Environment.Exit(1);
            }
        }

        static void InitializeRuntime_(AppSettings settings)
        {
            ConfigureLogger_(settings.LogProfile, settings.LogMinimumLevel);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;
        }

        static void ConfigureLogger_(string profile, LogEventLevel minimumLevel)
        {
            ILoggerProvider provider;

            using (var catalog = new AssemblyCatalog(typeof(Program).Assembly)) {
                using (var container = new CompositionContainer(catalog)) {
                    provider = container.GetExportedValue<ILoggerProvider>(profile);
                }
            }

            Log.Logger = provider.GetLogger(minimumLevel);
        }

        static void LogUnhandledException_(Exception exception)
        {
            Log.Fatal(Resources.UnhandledException, exception);
        }
    }
}

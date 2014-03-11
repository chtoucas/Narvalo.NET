// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using Autofac;
    using Narvalo.Narrative.Properties;
    using Serilog;

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
            InitializeLogging_();

            Log.Information(Resources.Starting);

            try {
                var settings = AppSettings.FromConfiguration();
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
            var result = new AppOptions();

            var parser = new CommandLine.Parser();
            parser.ParseArgumentsStrict(
                args,
                result,
                () => { throw new NarrativeException("Failed to parse the cmdline arguments."); });

            return result;
        }

        static IContainer CreateContainer_(AppSettings settings)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AppModule(settings));
            return builder.Build();
        }

        static void InitializeLogging_()
        {
            Log.Logger = new LoggerProvider().GetLogger();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;
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

        static void LogUnhandledException_(Exception exception)
        {
            Log.Fatal(Resources.UnhandledException, exception);
        }
    }
}

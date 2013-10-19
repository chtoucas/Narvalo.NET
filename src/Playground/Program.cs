using Autofac;

namespace Narvalo.Playground
{
    using System;
    using Narvalo.Autofac;
    using Narvalo.Diagnostics;
    using Narvalo.Playground.Properties;
    using Serilog;

    public class Program : AutofacProgram
    {
        //static readonly Lazy<ILogger> Logger_
        //    = new Lazy<ILogger>(() => Logger.Create(typeof(Program).Namespace));

        Program() : base() { }

        [STAThread]
        static void Main(string[] args)
        {
            InitializeRuntime_();

            //var logger = Logger_.Value;

            //logger.Log(LoggerLevel.Informational, Resources.Starting);

            try {
                new Program().Run();
            }
            catch (Exception ex) {
                LogUnhandledException_(ex);
            }

            //logger.Log(LoggerLevel.Informational, Resources.Ending);
        }

        protected override IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AppModule());
            return builder.Build();
        }

        #region Membres privés

        static void OnUnhandledException_(object sender, UnhandledExceptionEventArgs args)
        {
            try {
                LogUnhandledException_((Exception)args.ExceptionObject);
            }
            finally {
                Environment.Exit(1);
            }
        }

        static void InitializeRuntime_()
        {
            //log4net.Config.XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;
        }

        static void LogUnhandledException_(Exception ex)
        {
            //if (Logger_.IsValueCreated) {
            //    Logger_.Value.Log(LoggerLevel.Fatal, Resources.UnhandledException, ex);
            //}
        }

        #endregion
    }
}

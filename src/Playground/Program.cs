using Autofac;

namespace Playground
{
    using System;
    using Playground.Properties;
    using Serilog;

    public class Program
    {
        Program() { }

        [STAThread]
        static void Main(string[] args)
        {
            InitializeRuntime_();

            Log.Information(Resources.Starting);

            try {
                new Program().Run();
            }
            catch (Exception ex) {
                LogUnhandledException_(ex);
            }

            Log.Information(Resources.Ending);
        }

        public void Run()
        {
            using (var container = CreateContainer_()) {
            }
        }

        #region Membres privés

        static IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

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

        static void InitializeRuntime_()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException_;
        }

        static void LogUnhandledException_(Exception ex)
        {
            Log.Fatal(Resources.UnhandledException, ex);
        }

        #endregion
    }
}

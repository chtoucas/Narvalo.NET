namespace Playground.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Narvalo.Mvp.Binder;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var strategies = new List<IPresenterDiscoveryStrategy>();
            strategies.Add(new ConventionBasedPresenterDiscoveryStrategy());
            strategies.Add(new AttributeBasedPresenterDiscoveryStrategy());

            PresenterDiscoveryStrategyBuilder.Current.SetFactory(
                new CompositePresenterDiscoveryStrategy(strategies));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

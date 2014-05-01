namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Binder;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            new MvpBootstrapper()
                .DiscoverPresenter.With(
                    new AttributeBasedPresenterDiscoveryStrategy(),
                    new DefaultPresenterDiscoveryStrategy())
                .Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

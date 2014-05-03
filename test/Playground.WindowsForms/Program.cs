namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var bootstrapper = new FormsBootstrapper();

            bootstrapper.Configuration
                .DiscoverPresenter.With(
                    new DefaultFormPresenterDiscoveryStrategy(),
                    new AttributeBasedPresenterDiscoveryStrategy());

            bootstrapper.Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

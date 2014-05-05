namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var bootstrapper = new FormsMvpBootstrapper();
            bootstrapper.Configuration.MessageBusFactory.Is(new ReactiveMessageBusFactory());
            bootstrapper.Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

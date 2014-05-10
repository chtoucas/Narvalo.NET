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
            new MvpBootstrapper().Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

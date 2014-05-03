namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            new FormsMvpBootstrapper().Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

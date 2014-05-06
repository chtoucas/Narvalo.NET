namespace Playground.WebForms
{
    using System;
    using System.Web;
    using Narvalo.Mvp.Web;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AspNetMvpBootstrapper().Run();
        }
    }
}
namespace Playground.WebForms
{
    using System;
    using Narvalo.Mvp.Web;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AspNetMvpBootstrapper().Run();
        }
    }
}
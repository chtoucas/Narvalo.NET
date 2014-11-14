namespace MvpWebForms
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Web.Mvp;
    using Narvalo.Web.Mvp.Core;

    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            Trace.Listeners.Add(new WebPageTraceListener());

            var presenterDiscoveryStrategy
                = new AspNetConventionBasedPresenterDiscoveryStrategy(
                    AspNetConventionBasedPresenterDiscoveryStrategy.DefaultViewSuffixes,
                    new[] { "MvpWebForms.Presenters.{presenter}" },
                    enableCache: true);

            new MvpBootstrapper()
                .DiscoverPresenter.With(new AttributeBasedPresenterDiscoveryStrategy())
                .DiscoverPresenter.With(presenterDiscoveryStrategy)
                .Run();
        }
    }
}
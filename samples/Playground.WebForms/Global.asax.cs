namespace Playground
{
    using System;
    using System.Web;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Web;
    using Narvalo.Mvp.Web.Core;

    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            new MvpBootstrapper()
                .DiscoverPresenter.With(new CustomPresenterDiscoveryStrategy())
                .DiscoverPresenter.With(new AttributeBasedPresenterDiscoveryStrategy())
                .Run();
        }
    }

    public sealed class CustomPresenterDiscoveryStrategy
        : AspNetConventionBasedPresenterDiscoveryStrategy
    {
        static readonly string[] PresenterNameTemplates_ = new[]
        {
            "Playground.Presenters.{presenter}",
        };

        protected override string[] PresenterNameTemplates
        {
            get
            {
                return PresenterNameTemplates_;
            }
        }
    }
}
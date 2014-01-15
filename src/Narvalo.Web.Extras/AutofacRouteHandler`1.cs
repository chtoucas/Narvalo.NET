namespace Narvalo.Web
{
    using System.Web;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;

    // Cf. https://groups.google.com/forum/#!msg/autofac/BkY4s4tusUc/micDCB0YiN8J
    public class AutofacRouteHandler<THandler> : IRouteHandler where THandler : IHttpHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<THandler>();
        }
    }
}

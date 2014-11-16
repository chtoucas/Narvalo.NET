// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using Autofac;
using Autofac.Integration.Mvc;

namespace Narvalo.Autofac
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Routing;

    // Cf. https://groups.google.com/forum/#!msg/autofac/BkY4s4tusUc/micDCB0YiN8J
    public sealed class AutofacRouteHandler<THandler> : IRouteHandler where THandler : IHttpHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<THandler>();
        }
    }
}

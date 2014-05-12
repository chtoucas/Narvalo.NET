// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly string[] ViewSuffixes_ = new[] 
        {
            // Web Forms
            "UserControl",
            "Control",
            "Page",

            // Core ASP.NET
            "Handler",
            "WebService",
            "Service",

            // Generic
            "View",
        };

        static readonly string[] PresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly IPresenterDiscoveryStrategy _inner;

        public DefaultConventionBasedPresenterDiscoveryStrategy()
        {
            var typeResolver = new AspNetPresenterTypeResolver(
                new AspNetBuildManager(),
                // REVIEW
                Enumerable.Empty<string>(),
                ViewSuffixes_,
                PresenterNameTemplates_);

            _inner = new ConventionBasedPresenterDiscoveryStrategy(typeResolver);
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

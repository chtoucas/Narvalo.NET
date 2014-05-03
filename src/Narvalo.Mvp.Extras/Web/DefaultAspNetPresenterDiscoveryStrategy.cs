// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultAspNetPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
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

        public DefaultAspNetPresenterDiscoveryStrategy()
        {
             _inner = new ConventionBasedPresenterDiscoveryStrategy(
                new AspNetBuildManager(),
                // REVIEW
                Enumerable.Empty<string>(),
                ViewSuffixes_,
                PresenterNameTemplates_);
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts, 
            IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

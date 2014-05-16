// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;

    public class AspNetConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
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

        public AspNetConventionBasedPresenterDiscoveryStrategy()
        {
            var typeResolver = new AspNetPresenterTypeResolver(
                new AspNetBuildManager(),
                // REVIEW
                Enumerable.Empty<string>(),
                ViewSuffixes,
                PresenterNameTemplates);

            _inner = new ConventionBasedPresenterDiscoveryStrategy(typeResolver);
        }

        protected virtual string[] PresenterNameTemplates
        {
            get { return PresenterNameTemplates_; }
        }

        protected virtual string[] ViewSuffixes
        {
            get { return ViewSuffixes_; }
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;

    public class AspNetConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly string[] DefaultPresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        static readonly string[] DefaultViewSuffixes_ = new[] 
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

        readonly IPresenterDiscoveryStrategy _inner;

        public AspNetConventionBasedPresenterDiscoveryStrategy()
            : this(DefaultViewSuffixes_, DefaultPresenterNameTemplates_, enableCache: true) { }

        public AspNetConventionBasedPresenterDiscoveryStrategy(
            IEnumerable<string> viewSuffixes,
            IEnumerable<string> presenterNameTemplates,
            bool enableCache)
        {
            // REVIEW: Empty namespaces.
            var typeResolver = new AspNetPresenterTypeResolver(
                new AspNetBuildManager(),
                Enumerable.Empty<string>(),
                viewSuffixes,
                presenterNameTemplates);

            _inner = new ConventionBasedPresenterDiscoveryStrategy(typeResolver, enableCache);
        }

        public static IEnumerable<string> DefaultPresenterNameTemplates { get { return DefaultPresenterNameTemplates_; } }

        public static IEnumerable<string> DefaultViewSuffixes { get { return DefaultViewSuffixes_; } }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

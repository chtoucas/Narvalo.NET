// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    public class AspNetConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private static readonly string[] s_DefaultPresenterNameTemplates = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        private static readonly string[] s_DefaultViewSuffixes = new[]
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

        private readonly IPresenterDiscoveryStrategy _inner;

        public AspNetConventionBasedPresenterDiscoveryStrategy()
            : this(s_DefaultViewSuffixes, s_DefaultPresenterNameTemplates, enableCache: true) { }

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

            _inner = new PresenterDiscoveryStrategy(typeResolver, enableCache);
        }

        public static IEnumerable<string> DefaultPresenterNameTemplates
        {
            get { return s_DefaultPresenterNameTemplates; }
        }

        public static IEnumerable<string> DefaultViewSuffixes { get { return s_DefaultViewSuffixes; } }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    // Convention based presenter discovery strategy.
    public partial class AspNetPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
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

        public AspNetPresenterDiscoveryStrategy()
            : this(s_DefaultViewSuffixes, s_DefaultPresenterNameTemplates, enableCache: true) { }

        public AspNetPresenterDiscoveryStrategy(
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

        public static IEnumerable<string> DefaultPresenterNameTemplates => s_DefaultPresenterNameTemplates;

        public static IEnumerable<string> DefaultViewSuffixes => s_DefaultViewSuffixes;

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
            => _inner.FindBindings(hosts, views);
    }
}

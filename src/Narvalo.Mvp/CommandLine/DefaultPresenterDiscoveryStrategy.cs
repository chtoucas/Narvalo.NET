// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Resolvers;

    // Convention based presenter discovery strategy.
    public sealed partial class DefaultPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private static readonly string[] s_ViewSuffixes = new[]
        {
            "Command",
            "View",
        };

        private static readonly string[] s_PresenterNameTemplates = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        private readonly IPresenterDiscoveryStrategy _inner;

        public DefaultPresenterDiscoveryStrategy()
            : this(Assembly.GetEntryAssembly())
        { }

        public DefaultPresenterDiscoveryStrategy(Assembly assembly)
            : this(new[] { assembly })
        { }

        public DefaultPresenterDiscoveryStrategy(Assembly[] assemblies)
        {
            var typeResolver = new PresenterTypeResolver(
                   new BuildManager(assemblies),
                   assemblies.Select(_ => new AssemblyName(_.FullName).Name),
                   s_ViewSuffixes,
                   s_PresenterNameTemplates);

            _inner = new PresenterDiscoveryStrategy(typeResolver);
        }

        public PresenterDiscoveryResult FindBindings(IEnumerable<object> hosts, IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}

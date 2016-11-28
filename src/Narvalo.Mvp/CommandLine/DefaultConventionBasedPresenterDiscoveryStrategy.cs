// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Resolvers;

    public sealed class DefaultConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
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

        public DefaultConventionBasedPresenterDiscoveryStrategy()
            : this(Assembly.GetEntryAssembly()) { }

        public DefaultConventionBasedPresenterDiscoveryStrategy(Assembly assembly)
            : this(new[] { assembly }) { }

        public DefaultConventionBasedPresenterDiscoveryStrategy(Assembly[] assemblies)
        {
            Expect.NotNull(assemblies);

            var typeResolver = new PresenterTypeResolver(
                   new BuildManager(assemblies),
                   assemblies.Select(_ => new AssemblyName(_.FullName).Name),
                   s_ViewSuffixes,
                   s_PresenterNameTemplates);

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

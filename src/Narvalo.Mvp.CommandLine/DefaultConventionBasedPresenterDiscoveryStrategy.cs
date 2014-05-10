// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
        static readonly string[] ViewSuffixes_ = new[] 
        {
            "Command",
            "View",
        };

        static readonly string[] PresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly IPresenterDiscoveryStrategy _inner;

        public DefaultConventionBasedPresenterDiscoveryStrategy()
            : this(new[] { Assembly.GetEntryAssembly() }) { }

        public DefaultConventionBasedPresenterDiscoveryStrategy(Assembly[] assemblies)
        {
            var typeResolver = new PresenterTypeResolver(
                   new BuildManager(assemblies),
                   assemblies.Select(_ => new AssemblyName(_.FullName).Name),
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

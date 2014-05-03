// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultCommandPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
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

        public DefaultCommandPresenterDiscoveryStrategy()
            : this(new[] { Assembly.GetEntryAssembly() }) { }

        public DefaultCommandPresenterDiscoveryStrategy(Assembly[] assemblies)
        {
            _inner = new ConventionBasedPresenterDiscoveryStrategy(
                new BuildManager(assemblies),
                assemblies.Select(_ => new AssemblyName(_.FullName).Name),
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

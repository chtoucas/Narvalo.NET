// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal;
    using Narvalo.Mvp.Internal.Resolvers;

    public class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly string[] ViewSuffixes_ = new[] 
        {
            // Command Line
            "Command",

            // Windows Forms
            "UserControl",
            "Control",
            "Form",

            // Last chance
            "View",
        };

        static readonly string[] PresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly PresenterTypeResolver _typeResolver;

        public ConventionBasedPresenterDiscoveryStrategy()
            : this(
               new[] { Assembly.GetEntryAssembly() },
                ViewSuffixes_,
                PresenterNameTemplates_) { }

        public ConventionBasedPresenterDiscoveryStrategy(
            Assembly[] assemblies,
            string[] viewSuffixes,
            string[] presenterNameTemplates)
        {
            Require.NotNull(assemblies, "assemblies");
            Require.NotNull(viewSuffixes, "viewSuffixes");
            Require.NotNull(presenterNameTemplates, "presenterNameTemplates");

            var buildManager = new BuildManager(assemblies);
            var defaultNamespaces = assemblies.Select(_ => new AssemblyName(_.FullName).Name);

            _typeResolver = new CachedPresenterTypeResolver(
                buildManager,
                defaultNamespaces,
                viewSuffixes,
                presenterNameTemplates);
        }

        public ConventionBasedPresenterDiscoveryStrategy(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            string[] viewSuffixes,
            string[] presenterNameTemplates)
        {
            Require.NotNull(buildManager, "buildManager");
            Require.NotNull(defaultNamespaces, "defaultNamespaces");
            Require.NotNull(viewSuffixes, "viewSuffixes");
            Require.NotNull(presenterNameTemplates, "presenterNameTemplates");

            _typeResolver = new CachedPresenterTypeResolver(
                buildManager,
                defaultNamespaces,
                viewSuffixes,
                presenterNameTemplates);
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBinding>();

            foreach (var view in views) {
                var viewType = view.GetType();
                var presenterType = _typeResolver.Resolve(viewType);

                if (presenterType != null) {
                    __Trace.Write(
                        "Found a default presenter type: {0} for view type: {1})",
                        presenterType.FullName,
                        viewType.FullName
                    );

                    var binding = new PresenterBinding(
                        presenterType,
                        viewType,
                        PresenterBindingMode.Default,
                        new[] { view });

                    bindings.Add(binding);
                    boundViews.Add(view);
                }
                else {
                    __Trace.Write(
                        "No default presenter type found for view type: {0})",
                        viewType.FullName
                    );
                }
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }
    }
}

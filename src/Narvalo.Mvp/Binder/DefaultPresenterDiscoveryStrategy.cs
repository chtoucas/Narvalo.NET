// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal;
    using Narvalo.Mvp.Internal.Resolvers;

    public class DefaultPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IList<string> ViewSuffixes_ = new[] 
        {
            "UserControl",
            "Control",
            // Windows Forms
            "Form",
            // Web Forms
            "Page",
            "Handler",
            "WebService",
            "Service",
            // Last chance
            "View",
        };

        static readonly IList<string> PresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly PresenterTypeResolver _typeResolver;

        public DefaultPresenterDiscoveryStrategy()
            : this(ViewSuffixes_, PresenterNameTemplates_) { }

        public DefaultPresenterDiscoveryStrategy(
            IList<string> viewSuffixes,
            IList<string> presenterNameTemplates)
        {
            _typeResolver = new CachedPresenterTypeResolver(viewSuffixes, presenterNameTemplates);
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

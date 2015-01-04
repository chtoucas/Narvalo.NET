// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    public sealed class /*Default*/CompositeViewFactory : ICompositeViewFactory
    {
        readonly ICompositeViewTypeResolver _typeResolver;

        public CompositeViewFactory()
            : this(new CompositeViewTypeResolver()) { }

        public CompositeViewFactory(ICompositeViewTypeResolver typeResolver)
            : this(typeResolver, true) { }

        public CompositeViewFactory(
            ICompositeViewTypeResolver typeResolver,
            bool enableCache)
        {
            Require.NotNull(typeResolver, "typeResolver");

            _typeResolver = enableCache
                 ? new CachedCompositeViewTypeResolver(typeResolver)
                 : typeResolver;
        }

        public ICompositeView Create(Type viewType, IEnumerable<IView> views)
        {
            Require.NotNull(viewType, "viewType");
            Require.NotNull(views, "views");

            var compositeViewType = _typeResolver.Resolve(viewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);

            foreach (var item in views) {
                view.Add(item);
            }

            return view;
        }
    }
}
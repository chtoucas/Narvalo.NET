// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    public sealed class DefaultCompositeViewFactory : ICompositeViewFactory
    {
        readonly ICompositeViewTypeResolver _typeResolver;

        public DefaultCompositeViewFactory()
            : this(new CompositeViewTypeResolver()) { }

        public DefaultCompositeViewFactory(ICompositeViewTypeResolver typeResolver)
        {
            Require.NotNull(typeResolver, "typeResolver");

            _typeResolver = new CachedCompositeViewTypeResolver(typeResolver);
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
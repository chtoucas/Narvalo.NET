// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp.Internal.Providers;

    public sealed class DefaultCompositeViewFactory : ICompositeViewFactory
    {
        readonly CompositeViewTypeProvider _compositeViewTypeProvider
            = new CachedCompositeViewTypeProvider();

        public ICompositeView Create(Type viewType, IEnumerable<IView> views)
        {
            Require.NotNull(viewType, "viewType");
            Require.NotNull(views, "views");

            var compositeViewType = _compositeViewTypeProvider.GetComponent(viewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);

            foreach (var item in views) {
                view.Add(item);
            }

            return view;
        }
    }
}
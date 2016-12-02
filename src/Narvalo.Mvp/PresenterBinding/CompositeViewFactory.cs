// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    public sealed class /*Default*/CompositeViewFactory : ICompositeViewFactory
    {
        private readonly ICompositeViewTypeResolver _typeResolver;

        public CompositeViewFactory()
            : this(new CompositeViewTypeResolver()) { }

        public CompositeViewFactory(ICompositeViewTypeResolver typeResolver)
            : this(typeResolver, true)
        {
            Expect.NotNull(typeResolver);
        }

        public CompositeViewFactory(
            ICompositeViewTypeResolver typeResolver,
            bool enableCache)
        {
            Require.NotNull(typeResolver, nameof(typeResolver));

            _typeResolver = enableCache
                 ? new CachedCompositeViewTypeResolver(typeResolver)
                 : typeResolver;
        }

        public ICompositeView Create(Type viewType, IEnumerable<IView> views)
        {
            Require.NotNull(viewType, nameof(viewType));
            Require.NotNull(views, nameof(views));

            var compositeViewType = _typeResolver.Resolve(viewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);

            foreach (var item in views)
            {
                view.Add(item);
            }

            return view;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_typeResolver != null);
        }

#endif
    }
}
// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Generic;

    internal sealed class CachedPresenterTypeProvider : PresenterTypeProvider
    {
        readonly TypeKeyedProviderCache<Type> _cache = new TypeKeyedProviderCache<Type>();

        public CachedPresenterTypeProvider(
            IList<string> viewInstanceSuffixes,
            IList<string> candidatePresenterNames)
            : base(viewInstanceSuffixes, candidatePresenterNames) { }

        public override Type GetComponent(Type input)
        {
            return _cache.GetOrAdd(input, base.GetComponent);
        }
    }
}

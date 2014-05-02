// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;
    using System.Collections.Generic;

    internal sealed class CachedPresenterTypeResolver : PresenterTypeResolver
    {
        readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        public CachedPresenterTypeResolver(
            IList<string> viewInstanceSuffixes,
            IList<string> candidatePresenterNames)
            : base(viewInstanceSuffixes, candidatePresenterNames) { }

        public override Type Resolve(Type input)
        {
            return _cache.GetOrAdd(input, base.Resolve);
        }
    }
}

// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;
    using System.Collections.Generic;

    internal sealed class CachedPresenterTypeResolver : PresenterTypeResolver
    {
        readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        public CachedPresenterTypeResolver(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            string[] viewInstanceSuffixes,
            string[] candidatePresenterNames)
            : base(buildManager, defaultNamespaces, viewInstanceSuffixes, candidatePresenterNames) { }

        public override Type Resolve(Type input)
        {
            return _cache.GetOrAdd(input, base.Resolve);
        }
    }
}

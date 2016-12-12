// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Mvp.Properties;

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes",
           Justification = "Using three generic parameters does not seem that much!")]
    public partial class ResolverCache<TKey, TCacheKey, TValue> where TKey : class
    {
        private readonly ConcurrentDictionary<TCacheKey, TValue> _dictionary
           = new ConcurrentDictionary<TCacheKey, TValue>();

        private readonly Func<TKey, TCacheKey> _cacheKeyProvider;

        public ResolverCache(Func<TKey, TCacheKey> cacheKeyProvider)
        {
            Require.NotNull(cacheKeyProvider, nameof(cacheKeyProvider));

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));
            Expect.NotNull(key);
            Warrant.NotNullUnconstrained<TValue>();

            TCacheKey innerKey = _cacheKeyProvider.Invoke(key);

            if (innerKey == null)
            {
                throw new InvalidOperationException(Strings.ResolverCache_KeyFactoryReturnsNull);
            }

            var value = _dictionary.GetOrAdd(innerKey, _ => valueFactory.Invoke(key));

            if (value == null)
            {
                throw new ArgumentException(Strings.ResolverCache_ValueFactoryReturnsNull, nameof(valueFactory));
            }

            return value;
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System.Diagnostics.Contracts;

    public partial class ResolverCache<TKey, TCacheKey, TValue>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_cacheKeyProvider != null);
            Contract.Invariant(_dictionary != null);
        }
    }
}

#endif

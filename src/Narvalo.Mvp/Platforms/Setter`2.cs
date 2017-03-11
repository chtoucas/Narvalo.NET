// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;

    public sealed partial class Setter<TSource, T> where TSource : class
    {
        private readonly TSource _source;
        private readonly Action<T> _set;

        public Setter(TSource source, Action<T> set)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(set, nameof(set));

            _source = source;
            _set = set;
        }

        public TSource Is(T value)
        {
            _set.Invoke(value);

            return _source;
        }
    }
}
